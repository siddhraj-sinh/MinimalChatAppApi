namespace MinimalChatAppApi.Models
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public UserProfileDto Profile { get; set; }
    }
}
