using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Messages
{

    public class ChatHub(ApplicationDbContext context) : Hub
    {
        private readonly ApplicationDbContext context = context;

        public async Task SendMessage(Guid chatId, string message)
        {
            if (!Guid.TryParse(Context.UserIdentifier, out Guid senderId))
            {
                throw new Exception("Invalid user identifier");
            }

            var chat = await context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ChatId == chatId)
                ?? throw new Exception("Chat not found");

            var receiverId = chat.Participant1Id == senderId
                ? chat.Participant2Id
                : chat.Participant1Id;

            var newMessage = new Message
            {
                ChatId = chatId,
                SenderId = senderId,  
                ReceiverId = receiverId,
                Content = message,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            chat.LastMessageAt = DateTime.UtcNow;
            context.Messages.Add(newMessage);
            await context.SaveChangesAsync();

            // Convert receiverId to string for SignalR
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatId, senderId, message);
        }
    }

}
