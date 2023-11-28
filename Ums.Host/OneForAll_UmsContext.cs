using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ums.Domain.AggregateRoots;

namespace Ums.Host
{
    public partial class OneForAll_UmsContext : DbContext
    {
        public OneForAll_UmsContext(DbContextOptions<OneForAll_UmsContext> options)
            : base(options)
        {

        }
        public virtual DbSet<UmsMessage> UmsMessage { get; set; }
        public virtual DbSet<UmsMessageRecord> UmsMessageRecord { get; set; }
        public virtual DbSet<UmsSmsRecord> UmsSmsRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UmsMessage>(entity =>
            {
                entity.ToTable("Ums_Message");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UmsMessageRecord>(entity =>
            {
                entity.ToTable("Ums_MessageRecord");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UmsSmsRecord>(entity =>
            {
                entity.ToTable("Ums_SmsRecord");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
