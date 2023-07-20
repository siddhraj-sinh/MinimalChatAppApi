namespace MinimalChatAppApi.Models
{
    public class ConversationDto
    {
        public int UserId { get; set; }
        public DateTime? Before { get; set; }
        public int Count { get; set; } = 20; // Default value is 20
        public string Sort { get; set; } = "asc"; // Default value is "asc"
    }
}
