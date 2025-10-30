using CorrespondenceTracker.Data;
using CorrespondenceTracker.Infrastructure.BackgroundServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// 1. Apply migrations for CorrespondenceDatabaseContext
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CorrespondenceDatabaseContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
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