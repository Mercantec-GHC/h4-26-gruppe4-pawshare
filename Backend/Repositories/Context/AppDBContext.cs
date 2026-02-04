using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        public DbSet<Animal> Animals { get; set; } = default!;
        public DbSet<AnimalType> AnimalTypes { get; set; } = default!;
        public DbSet<Appointment> Appointments { get; set; } = default!;
        public DbSet<Chat> Chats { get; set; } = default!;
        public DbSet<Message> Messages { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<AppointmentAnimalBooking> AppointmentAnimalBookings { get; set; } = default!;
        public DbSet<ChatUserConvo> ChatUserConvos { get; set; } = default!;
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatUserConvo>()
                .HasKey(x => new { x.UserId, x.ChatId });

            modelBuilder.Entity<ChatUserConvo>()
                .HasOne(x => x.User)
                .WithMany(u => u.Chats)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<ChatUserConvo>()
                .HasOne(x => x.Chat)
                .WithMany(c => c.ChatUsers)
                .HasForeignKey(x => x.ChatId);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "AnimalUser" },
                new Role { Id = 3, Name = "Institution" },
                new Role { Id = 4, Name = "Moderator" }
            );

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Common>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
