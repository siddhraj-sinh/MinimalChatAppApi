using Microsoft.EntityFrameworkCore;
using MinimalChatAppApi.Data;
using MinimalChatAppApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MinimalChatAppApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly IDbContextFactory<ChatContext> _contextFactory;
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IDbContextFactory<ChatContext> contextFactory)
        {
            _next = next;
            _contextFactory = contextFactory;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            using var dbContext = _contextFactory.CreateDbContext();

            httpContext.Request.EnableBuffering();
            var requestBody = await new System.IO.StreamReader(httpContext.Request.Body).ReadToEndAsync();
            httpContext.Request.Body.Position = 0;

            var currentUser = httpContext.User;
            // Access user properties
            var userName = currentUser.FindFirst(ClaimTypes.Name)?.Value;

            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            var timeStamp = DateTimeOffset.UtcNow;

            // Log the request details
            var logEntry = new LogModel
            {
                Timestamp = DateTime.Now,
                IpAddress = ipAddress,
                Username = userName,
                RequestBody = requestBody
            };

            Console.WriteLine(userName);
            dbContext.Log.Add(logEntry);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation(logEntry.ToString());

            await _next(httpContext);
        }
    }
}
