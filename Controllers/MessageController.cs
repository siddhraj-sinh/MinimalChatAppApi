using System;
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

        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Message/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Message
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
