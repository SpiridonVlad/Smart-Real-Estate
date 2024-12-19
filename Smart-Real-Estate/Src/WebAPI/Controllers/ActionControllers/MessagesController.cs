using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Application.Messages;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Persistence;

[ApiController]
[Route("api/messages")]
public class MessagesController(IHubContext<ChatHub> chatHub, IConfiguration configuration) : ControllerBase
{
    private readonly IHubContext<ChatHub> chatHub = chatHub;

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
    {
        throw new NotImplementedException();
    }
}