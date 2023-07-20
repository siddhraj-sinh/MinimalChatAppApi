namespace MinimalChatAppApi.Models
{
    public class LogModel
    {
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string RequestBody { get; set; }
        // Constructor


        // Override ToString method
        public override string ToString()
        {
            return $"Timestamp: {Timestamp}\n" +
                   $"IP Address: {IpAddress}\n" +
                   $"Username: {Username}\n" +
                   $"Request Body: {RequestBody}";
        }
    }
}
