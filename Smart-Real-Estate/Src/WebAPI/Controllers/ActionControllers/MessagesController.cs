using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Utils;
using Domain.Entities.Messages;
using Application.Messages;


namespace Real_Estate_Management_System.Controllers.ActionControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext context = context;

        [HttpPost("Send/{chatId}")]
        public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] SendMessageCommand request)
        {
            var currentUserId = JwtHelper.GetUserIdFromJwt(HttpContext).Data;

            var chat = await context.Chats
                .FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (chat == null)
                return NotFound("Chat not found.");

            // Ensure the user is a participant in the chat
            if (chat.Participant1Id != currentUserId && chat.Participant2Id != currentUserId)
                return Forbid("You are not a participant in this chat.");

            // Create the new message
            var message = new Domain.Entities.Message
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                SenderId = currentUserId,
                ReceiverId = (chat.Participant1Id == currentUserId) ? chat.Participant2Id : chat.Participant1Id,
                Content = request.Content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            // Add the message to the database
            context.Messages.Add(message);
            chat.LastMessageAt = message.Timestamp; // Update the chat's last message timestamp
            await context.SaveChangesAsync();

            return Ok("Message sent successfully.");
        }

        [HttpPost("MarkAsRead/{chatId}")]
        public async Task<IActionResult> MarkMessagesAsRead(Guid chatId)
        {
            var currentUserId = JwtHelper.GetUserIdFromJwt(HttpContext).Data;

            var unreadMessages = await context.Messages
                .Where(m => m.ChatId == chatId && m.ReceiverId == currentUserId && !m.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }

            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Chat/{userId}")]
        public async Task<ActionResult<Chat>> GetOrCreateChat(Guid userId)
        {
            var userResult = JwtHelper.GetUserIdFromJwt(HttpContext);
            if (!userResult.IsSuccess)
            {
                return BadRequest(userResult.ErrorMessage);
            }
            var currentUserId = userResult.Data;

            var chat = await context.Chats
                .FirstOrDefaultAsync(c =>
                    c.Participant1Id == currentUserId && c.Participant2Id == userId ||
                    c.Participant1Id == userId && c.Participant2Id == currentUserId);

            if (chat == null)
            {
                chat = new Chat
                {
                    ChatId = Guid.NewGuid(),
                    Participant1Id = currentUserId,
                    Participant2Id = userId,
                    CreatedAt = DateTime.UtcNow,
                    Messages = []
                };
                context.Chats.Add(chat);
                await context.SaveChangesAsync();
            }

            return chat;
        }

        [HttpGet("Messages/{chatId}")]
        public async Task<ActionResult<IEnumerable<Domain.Entities.Message>>> GetChatMessages(Guid chatId)
        {
            var currentUserId = JwtHelper.GetUserIdFromJwt(HttpContext).Data;

            var chat = await context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (chat == null)
                return NotFound();

            // Security check
            if (chat.Participant1Id != currentUserId && chat.Participant2Id != currentUserId)
                return Forbid();

            return chat.Messages.OrderByDescending(m => m.Timestamp).ToList();
        }

        [HttpGet("Chats")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetUserChats()
        {
            var currentUserId = JwtHelper.GetUserIdFromJwt(HttpContext).Data;

            return await context.Chats
                .Where(c => c.Participant1Id == currentUserId || c.Participant2Id == currentUserId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

    }
}