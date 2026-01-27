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
        public DbSet<Post> Posts { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
    }
}
