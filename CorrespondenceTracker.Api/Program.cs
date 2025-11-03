using CorrespondenceTracker.Data;
using CorrespondenceTracker.Infrastructure.BackgroundServices;
using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var reportService = new LocalReporting().TempDirectory("jsreport/Temp")
         .UseBinary(JsReportBinary.GetBinary())
         .KillRunningJsReportProcesses()
         .AsWebServer()
         .Create();
        await reportService.StartAsync();
        builder.Services.AddJsReport(reportService);
        // Add services to the container
        builder.Services.AddControllers();
        //builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<RazorViewEngineOptions>(o =>
        {
            o.ViewLocationFormats.Clear();
            o.ViewLocationFormats.Add("/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
            o.ViewLocationFormats.Add("/Views/{0}" + RazorViewEngine.ViewExtension);
        });
        builder.Services.AddEndpointsApiExplorer();
        // 2. Automatically register interfaces and their matching implementations
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.Services.Scan(scan =>
        {
            scan
            .FromApplicationDependencies(d => d.FullName.StartsWith("CorrespondenceTracker"))
            .AddClasses(publicOnly: true)
            .AsMatchingInterface((service, filter) =>
                filter.Where(implementation => implementation.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase)))
            .WithTransientLifetime();
        });

        // Register DbContext
        builder.Services.AddDbContext<CorrespondenceDatabaseContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("CorrespondenceDatabase"))
                   .LogTo(Console.WriteLine, LogLevel.Warning);
        });

        // Register Background Service for Reminder Emails
        builder.Services.AddHostedService<ReminderEmailBackgroundService>();
        builder.Services.AddRazorPages();
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CorrespondenceDatabaseContext>();
            db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Correspondence Tracker API v1");
                options.RoutePrefix = string.Empty; // 👈 opens at the root URL
            });
        }
        app.UseCors(x => x.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                        );

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}