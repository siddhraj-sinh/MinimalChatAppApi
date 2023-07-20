namespace MinimalChatAppApi.Models
{
    public class ConversationResponseDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
    public class ConversationHistoryResponseDto
    {
        public IEnumerable<ConversationResponseDto> Messages { get; set; }
    }
}
