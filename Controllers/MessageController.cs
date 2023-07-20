﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalChatAppApi.Data;
using MinimalChatAppApi.Models;

namespace MinimalChatAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ChatContext _context;

        public MessageController(ChatContext context)
        {
            _context = context;
        }


        [HttpPost("/api/messages")]
        [Authorize]
        public async Task<IActionResult> SendMessages([FromBody] SendMessageDto message) {
            // Get the current user
            var currentUser = HttpContext.User;

            // Access user properties
            var senderId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the receiver user exists
            var receiverUser = await _context.Users.FindAsync(message.ReceiverId);
            if (receiverUser == null)
            {
                return BadRequest(new { error = "Receiver user not found" });
            }
            // Create the message object
            var newMessage = new Message
            {
                SenderId = Convert.ToInt32(senderId),
                ReceiverId = message.ReceiverId,
                MessageContent = message.Content,
                Timestamp = DateTime.UtcNow
            };

            // Save the message to the database
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Construct the response body
            var responseDto = new SendMessageResponseDto
            {
                MessageId = newMessage.Id,
                SenderId = newMessage.SenderId,
                ReceiverId = newMessage.ReceiverId,
                Content = newMessage.MessageContent,
                Timestamp = newMessage.Timestamp
            };

            // Return 200 OK with the response body
            return Ok(responseDto);

        }

        [HttpPut("/api/messages/{messageId}")]
        [Authorize]
        public async Task<IActionResult> EditMessage(int messageId, [FromBody] EditMessageDto messageDto)
        {
            // Get the current user
            var currentUser = HttpContext.User;

            // Access user properties
            var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Find the message in the database based on messageId and currentUserId
            var message = await _context.Messages
                .Where(m => m.Id == messageId && (m.SenderId == currentUserId))
                .SingleOrDefaultAsync();
            // Check if the message exists
            if (message == null)
            {
                return NotFound(new { error = "Message not found" });
            }

            // Check if the current user is the sender of the message
            if (message.SenderId != currentUserId)
            {
                return Unauthorized(new { error = "You can only edit your own messages" });
            }

            // Update the message content
            message.MessageContent = messageDto.Content;
            message.Timestamp = DateTime.UtcNow;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return 200 OK with a success message
            return Ok(new { message = "Message edited successfully" });
        }

        [HttpDelete("/api/messages/{messageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var currentUser = HttpContext.User;
            // Access user properties
            var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var message = await _context.Messages
               .Where(m => m.Id == messageId && (m.SenderId == currentUserId))
               .SingleOrDefaultAsync();

            // Check if the message exists
            if (message == null)
            {
                return NotFound(new { error = "Message not found" });
            }

            // Remove the message from the database
            _context.Messages.Remove(message);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return 200 OK with a success message
            return Ok(new { message = "Message deleted successfully" });
        }

       
    }
}
