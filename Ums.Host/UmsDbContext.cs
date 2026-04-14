using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ums.Domain.Entities;

namespace Ums.Host
{
    public partial class UmsDbContext : DbContext
    {
        public UmsDbContext(DbContextOptions<UmsDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<UmsMessage> UmsMessage { get; set; }
        public virtual DbSet<UmsMessageRecord> UmsMessageRecord { get; set; }
        public virtual DbSet<UmsSmsRecord> UmsSmsRecord { get; set; }
        public virtual DbSet<UmsNotificationConfig> UmsNotificationConfig { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UmsMessage>(entity =>
            {
                entity.ToTable("ums_message");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UmsMessageRecord>(entity =>
            {
                entity.ToTable("ums_message_record");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UmsSmsRecord>(entity =>
            {
                entity.ToTable("ums_sms_record");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UmsNotificationConfig>(entity =>
            {
                entity.ToTable("ums_notification_config");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
