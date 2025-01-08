using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMessagesRepository 
    {
        Task<Result<IEnumerable<Message>>> GetMessages(Guid chatId);
        Task<Result<Message>> GetMessage(Guid messageId);
        Task<Result<Guid>> CreateMessage(Message message);
        Task<Result<object>> UpdateMessage(Message message);
        Task<Result<object>> DeleteMessage(Guid messageId);
        Task<Result<IEnumerable<Message>>> GetMessagesByUser(Guid userId);
        Task<Result<Guid>> AddMessageToChat(Guid chatId, Guid messageId);
    }
}
