using Microsoft.EntityFrameworkCore;
using MinimalChatAppApi.Models;

namespace MinimalChatAppApi.Data
{
    public class ChatContext : DbContext
    {

        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure the foreign key relationship between Message.SenderId and User.Id
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction); // Specify no action on delete

            // Configure the foreign key relationship between Message.ReceiverId and User.Id
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction); // Specify no action on delete
        }
    }
}
