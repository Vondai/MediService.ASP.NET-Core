using MediService.ASP.NET_Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MediService.ASP.NET_Core.Data
{
    public class MediServiceDbContext : DbContext
    {

        public MediServiceDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; init; }

        public DbSet<Address> Addresses { get; init; }

        public DbSet<Appointment> Appointments { get; init; }
        public DbSet<Message> Messages { get; init; }

        public DbSet<Service> Services { get; init; }

        public DbSet<Subscription> Subscriptions { get; init; }

        public DbSet<Specialist> Specialists { get; init; }


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
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
