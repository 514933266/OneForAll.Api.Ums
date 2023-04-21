using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ums.Domain.AggregateRoots;

namespace Ums.Host
{
    public partial class OneForAllContext : DbContext
    {
        private Guid _tenantId;

        public OneForAllContext(DbContextOptions<OneForAllContext> options)
            : base(options)
        {

        }

        public OneForAllContext(
            DbContextOptions<OneForAllContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider.GetTenantId();
        }

        public virtual DbSet<UmsFailureMessageRecord> UmsFailureMessageRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UmsFailureMessageRecord>(entity =>
            {
                entity.ToTable("Ums_FailureMessageRecord");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
