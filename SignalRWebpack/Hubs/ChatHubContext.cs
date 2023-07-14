using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRWebpack.Global;
using SignalRWebpack.Models;
using System.Diagnostics;

namespace SignalRWebpack.Hubs;

/// <summary>
/// 서버가 시그널R과 통신하기위한 클래스
/// </summary>
public class ChatHubContext
{
    private readonly IHubContext<ChatHub> _hubContext;
    public ChatHubContext(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }


    /// <summary>
    /// 요청한 대상한테 메시지 전달
    /// </summary>
    /// <param name="signalRSendModel"></param>
    /// <returns></returns>
    public async Task SendUser_Name(
        string sName
        , SignalRSendModel signalRSendModel)
    {
        UserModel? findUserName
            = GlobalStatic.UserList.Find(sName);
        if (null != findUserName)
        {
            await this.SendUser(findUserName.ConnectionId, signalRSendModel);
        }

    }

    /// <summary>
    /// 지정한 대상한테 메시지 전달
    /// </summary>
    /// <param name="sConnectionId"></param>
    /// <param name="signalRSendModel"></param>
    /// <returns></returns>
    public async Task SendUser(
        string sConnectionId
        , SignalRSendModel signalRSendModel)
    {
        string sSendModel = JsonConvert.SerializeObject(signalRSendModel);

        await _hubContext.Clients
            .Client(sConnectionId)
            .SendAsync("ReceiveMessage", sSendModel);
    }

    /// <summary>
    /// 모든 접속자에게 메시지 전달
    /// </summary>
    /// <param name="signalRSendModel"></param>
    /// <returns></returns>
    public async Task SendUser_All(SignalRSendModel signalRSendModel)
    {
        string sSendModel = JsonConvert.SerializeObject(signalRSendModel);

        await _hubContext.Clients
            .All
            .SendAsync("ReceiveMessage", sSendModel);
    }
}
