using CorrespondenceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Classification> Classifications { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<FileRecord> FileRecords { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

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
            modelBuilder.Entity<Classification>().ToTable("Classifications");
            modelBuilder.Entity<Subject>().ToTable("Subjects");
            modelBuilder.Entity<Reminder>().ToTable("Reminders");

            // Correspondences configuration
            modelBuilder.Entity<Correspondence>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.IncomingNumber).HasMaxLength(50);
                b.Property(x => x.OutgoingNumber).HasMaxLength(50);

                b.HasOne(x => x.Correspondent)
                    .WithMany()
                    .HasForeignKey(x => x.CorrespondentId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.Classifications)
                 .WithMany(c => c.Correspondences);

                b.HasOne(x => x.Subject)
                  .WithMany()
                  .HasForeignKey(x => x.SubjectId)
                  .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.AssignedUser)
                    .WithMany()
                    .HasForeignKey(x => x.AssignedUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.File)
                    .WithMany()
                    .HasForeignKey(x => x.FileId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Indexes for better performance
                b.HasIndex(x => x.IncomingNumber);
                b.HasIndex(x => x.IncomingDate);
                b.HasIndex(x => x.OutgoingNumber);
                b.HasIndex(x => x.OutgoingDate);
                b.HasIndex(x => x.Direction);
            });

            // FollowUps configuration
            modelBuilder.Entity<FollowUp>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Details).HasMaxLength(4000);
                b.HasOne(x => x.Correspondence)
                    .WithMany(x => x.FollowUps)
                    .HasForeignKey(x => x.CorrespondenceId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.FileRecord)
                    .WithMany()
                    .HasForeignKey(x => x.FileRecordId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
            });

            // Attachments configuration
            modelBuilder.Entity<Attachment>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(500);
                b.Property(x => x.Note);
                b.HasOne(x => x.Correspondence)
                    .WithMany(x => x.Attachments)
                    .HasForeignKey(x => x.CorrespondenceId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.FileRecord)
                    .WithMany()
                    .HasForeignKey(x => x.FileRecordId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Departments configuration
            modelBuilder.Entity<Department>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<Reminder>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Message).HasMaxLength(500);
                b.HasIndex(x => x.RemindTime);
            });

            // Users configuration
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FullName).IsRequired().HasMaxLength(100);
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
            modelBuilder.Entity<FileRecord>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FileName).IsRequired().HasMaxLength(500);
                b.Property(x => x.FullPath).IsRequired().HasMaxLength(1000);
                b.Property(x => x.ContentType).HasMaxLength(30);
                b.Property(x => x.Extension).HasMaxLength(10);
                b.Property(x => x.Size);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}