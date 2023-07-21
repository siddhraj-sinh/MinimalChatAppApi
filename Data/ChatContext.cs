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

        public DbSet<LogModel> Log { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //configure table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<LogModel>().ToTable("Logs");



            //configure receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
           //configure sender
            modelBuilder.Entity<Message>()
              .HasOne(m => m.Sender)
              .WithMany()
              .HasForeignKey(m => m.SenderId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
