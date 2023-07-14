using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRWebpack.Global;
using SignalRWebpack.Models;
using System.Diagnostics;

namespace SignalRWebpack.Hubs;

public class ChatHub : Hub
{
    
    public ChatHub()
    {
    }


    public override Task OnConnectedAsync()
    {
        //Guid로 id를 생성할 필요가 없다. 
        //Console.WriteLine("--> Connection Established" + Context.ConnectionId); 
        Debug.WriteLine("--> Connection Established : " + Context.ConnectionId);
        Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnID", Context.ConnectionId);

        //유저 리스트에 추가
        GlobalStatic.UserList.Add(Context.ConnectionId);

        return base.OnConnectedAsync();
    }

    /// <summary>
    /// 접속 끊김 처리
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        //유저 리스트에 제거
        GlobalStatic.UserList.Remove(Context.ConnectionId);
        Debug.WriteLine("Disconnected : " + Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 클라이언트에서 서버로 전달된 메시지 처리
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendMessageAsync(string message)
    {
        Debug.WriteLine("Message Received on: " + Context.ConnectionId);

        SignalRSendModel? sendModel
            = JsonConvert.DeserializeObject<SignalRSendModel>(message);
        
        if(null == sendModel)
        {
            return;
        }

        switch(sendModel.Command)
        {
            case "Login"://아이디 입력 요청
                {
                    UserModel? findUserName
                        = GlobalStatic.UserList.Find(sendModel.Message);
                    if(null != findUserName)
                    {
                        await this.SendUser_Me(new SignalRSendModel()
                        {
                            Sender = "server"
                            , Command = "LoginError_Duplication"
                            , Message = "이미 사용중인 아이디 입니다."
                        });

                        await this.OnDisconnectedAsync(null);
                    }
                    else
                    {
                        findUserName = GlobalStatic.UserList.FindConnectionId(Context.ConnectionId);
                        if(null != findUserName)
                        {
                            //전달받은 이름을 넣고
                            findUserName.Name = sendModel.Message;

                            await this.SendUser_Me(new SignalRSendModel()
                            {
                                Sender = "server"
                                , Command = "LoginSuccess"
                                , Message = findUserName.Name
                            });
                        }
                        else
                        {
                            //여기서 일치하는게 없다는건 리스트에추가되지 않았다는 의미이므로
                            //재접속을 요구한다.

                            await this.SendUser_Me(new SignalRSendModel()
                            {
                                Sender = "server"
                                , Command = "LoginError_Reconnect"
                                , Message = "다시 접속해 주세요"
                            });

                            await this.OnDisconnectedAsync(null);
                        }
                    }
                }
                break;


            case "MsgSend"://메시지 전달 요청
                {
                    UserModel? findUserName 
                        = GlobalStatic.UserList.FindConnectionId(Context.ConnectionId);

                    if(null != findUserName)
                    {
                        sendModel.Sender = findUserName.Name;

                        if (sendModel.To == string.Empty)
                        {
                            await this.SendUser_All(sendModel);
                        }
                        else
                        {
                            UserModel? findToUserName
                                = GlobalStatic.UserList.Find(sendModel.To);
                            if(null != findToUserName)
                            {
                                await this.SendUser(findToUserName.ConnectionId, sendModel);
                            }
                        }
                    }
                }
                break;

        }

    }

    /// <summary>
    /// 요청한 대상한테 메시지 전달
    /// </summary>
    /// <param name="signalRSendModel"></param>
    /// <returns></returns>
    private async Task SendUser_Me(SignalRSendModel signalRSendModel)
    {
        await this.SendUser(Context.ConnectionId, signalRSendModel);
    }

    /// <summary>
    /// 지정된 이름의 유저를 찾아 메시지를 전달한다.
    /// </summary>
    /// <param name="sName"></param>
    /// <param name="signalRSendModel"></param>
    /// <returns></returns>
    private async Task SendUser_Name(
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
    private async Task SendUser(
        string sConnectionId
        , SignalRSendModel signalRSendModel)
    {
        string sSendModel = JsonConvert.SerializeObject(signalRSendModel);

        await Clients
            .Client(sConnectionId)
            .SendAsync("ReceiveMessage", sSendModel);
    }

    /// <summary>
    /// 모든 접속자에게 메시지 전달
    /// </summary>
    /// <param name="signalRSendModel"></param>
    /// <returns></returns>
    private async Task SendUser_All(SignalRSendModel signalRSendModel)
    {
        string sSendModel = JsonConvert.SerializeObject(signalRSendModel);

        await Clients
            .All
            .SendAsync("ReceiveMessage", sSendModel);
    }
}
