namespace MinimalChatAppApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }

}
