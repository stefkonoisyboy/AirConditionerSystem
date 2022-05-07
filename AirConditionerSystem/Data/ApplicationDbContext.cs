using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirConditionerSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ServiceRequest>()
                .HasOne(s => s.Creator)
                .WithMany(s => s.CreatedRequests)
                .HasForeignKey(s => s.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ServiceRequest>()
                .HasOne(s => s.VisitedBy)
                .WithMany(s => s.VisitedRequests)
                .HasForeignKey(s => s.VisitedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
