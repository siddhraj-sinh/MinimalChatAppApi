using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalChatAppApi.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sender")]
        [Required]
        public int SenderId { get; set; }
        [ForeignKey("Receiver")]
        [Required]
        public int ReceiverId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }

}
