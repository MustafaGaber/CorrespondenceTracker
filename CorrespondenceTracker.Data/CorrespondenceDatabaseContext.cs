using CorrespondenceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace CorrespondenceTracker.Data
{
    public class CorrespondenceDatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CorrespondenceDatabaseContext(DbContextOptions<CorrespondenceDatabaseContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // Alternative constructor for dependency injection
        public CorrespondenceDatabaseContext(DbContextOptions<CorrespondenceDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Correspondence> Correspondences { get; set; }
        public DbSet<FollowUp> FollowUps { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<Domain.Entities.FileRecord> FileRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This will typically be configured via dependency injection
                // But this provides a fallback
                var connectionString = _configuration?.GetConnectionString("CorrespondenceDatabase");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table names and schema
            modelBuilder.Entity<Correspondence>().ToTable("Correspondences");
            modelBuilder.Entity<FollowUp>().ToTable("FollowUps");
            modelBuilder.Entity<Attachment>().ToTable("Attachments");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Correspondent>().ToTable("Correspondents");
            modelBuilder.Entity<FileRecord>().ToTable("FileRecords");

            // DateOnly converter
            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),
                dt => DateOnly.FromDateTime(dt)
            );

            var nullableDateOnlyConverter = new ValueConverter<DateOnly?, DateTime?>(
                d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : null,
                dt => dt.HasValue ? DateOnly.FromDateTime(dt.Value) : null
            );

            // Correspondences configuration
            modelBuilder.Entity<Correspondence>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.IncomingNumber).IsRequired().HasMaxLength(200);
                b.Property(x => x.IncomingDate).HasConversion(dateOnlyConverter).IsRequired();
                b.Property(x => x.OutgoingNumber).HasMaxLength(200);
                b.Property(x => x.OutgoingDate).HasConversion(nullableDateOnlyConverter);
                b.Property(x => x.Content).HasColumnType("NVARCHAR(MAX)"); // SQL Server large text
                b.Property(x => x.Summary).HasMaxLength(2000);
                b.Property(x => x.Direction).IsRequired();
                b.Property(x => x.PriorityLevel).IsRequired();

                b.HasOne(x => x.Correspondent)
                    .WithMany()
                    .HasForeignKey(x => x.CorrespondentId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.AssignedUser)
                    .WithMany()
                    .HasForeignKey(x => x.AssignedUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.MainFile)
                    .WithMany()
                    .HasForeignKey(x => x.MainFileId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Indexes for better performance
                b.HasIndex(x => x.IncomingNumber);
                b.HasIndex(x => x.IncomingDate);
                b.HasIndex(x => x.CorrespondentId);
                b.HasIndex(x => x.Direction);
            });

            // FollowUps configuration
            modelBuilder.Entity<FollowUp>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Date).HasConversion(dateOnlyConverter).IsRequired();
                b.Property(x => x.Details).HasMaxLength(4000);
                b.HasOne(x => x.Correspondence)
                    .WithMany(x => x.FollowUps)
                    .HasForeignKey(x => x.CorrespondenceId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.FileRecord)
                    .WithMany()
                    .HasForeignKey(x => x.FileRecordId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasIndex(x => x.CorrespondenceId);
                b.HasIndex(x => x.Date);
            });

            // Attachments configuration
            modelBuilder.Entity<Attachment>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(500);
                b.Property(x => x.Note).HasMaxLength(2000);
                b.Property(x => x.Date).HasConversion(nullableDateOnlyConverter);
                b.HasOne(x => x.Correspondence)
                    .WithMany(x => x.Attachments)
                    .HasForeignKey(x => x.CorrespondenceId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.FileRecord)
                    .WithMany()
                    .HasForeignKey(x => x.FileRecordId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(x => x.CorrespondenceId);
            });

            // Departments configuration
            modelBuilder.Entity<Department>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.HasIndex(x => x.Name).IsUnique();
            });

            // Users configuration
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FullName).IsRequired().HasMaxLength(300);
                b.Property(x => x.JobTitle).HasMaxLength(200);
                b.HasIndex(x => x.FullName);
            });

            // Correspondents configuration
            modelBuilder.Entity<Correspondent>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(300);
                b.HasIndex(x => x.Name);
            });

            // FileRecords configuration
            modelBuilder.Entity<Domain.Entities.FileRecord>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FileName).IsRequired().HasMaxLength(500);
                b.Property(x => x.RelativePath).IsRequired().HasMaxLength(1000);
                b.Property(x => x.ContentType).HasMaxLength(200);
                b.Property(x => x.Extension).HasMaxLength(20);
                b.Property(x => x.Size);
                b.Property(x => x.CreatedAt).IsRequired();

                b.HasIndex(x => x.CreatedAt);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}