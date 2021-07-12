using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.ASP.NET_Core.Data
{
    public class MediServiceDbContext : IdentityDbContext<User>
    {

        public MediServiceDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Address> Addresses { get; init; }

        public DbSet<Appointment> Appointments { get; init; }
        public DbSet<Message> Messages { get; init; }

        public DbSet<Service> Services { get; init; }

        public DbSet<Subscription> Subscriptions { get; init; }

        public DbSet<Specialist> Specialists { get; init; }

        public DbSet<Review> Reviews { get; init; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(a => a.Appointments)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany(a => a.Appointments)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Specialist)
                .WithOne(x => x.User)
                .HasForeignKey<Specialist>(x => x.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
