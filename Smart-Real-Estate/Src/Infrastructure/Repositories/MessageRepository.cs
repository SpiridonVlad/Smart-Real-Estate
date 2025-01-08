using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class MessageRepository(ApplicationDbContext context) : IMessagesRepository
    {
        private readonly ApplicationDbContext context = context;
        public async Task<Result<Guid>> AddMessageToChat(Guid chatId, Guid messageId)
        {
            throw new NotImplementedException();            
              
        }

        public async Task<Result<Guid>> CreateMessage(Message message)
        {
           throw new NotImplementedException();
        }

        public async Task<Result<object>> DeleteMessage(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Message>> GetMessage(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Message>>> GetMessages(Guid chatId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Message>>> GetMessagesByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> UpdateMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
