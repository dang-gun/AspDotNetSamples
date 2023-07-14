using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRWebpack.Global;
using SignalRWebpack.Hubs;
using SignalRWebpack.Models;

namespace SignalRWebpack.Controllers;

/// <summary>
/// 테스트용 API
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : Controller
{
    private readonly ChatHubContext _ChatHubContext;

    public TestController(IHubContext<ChatHub> hubContext)
    {
        this._ChatHubContext = new ChatHubContext(hubContext);
    }


    /// <summary>
    /// 무조건 성공
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult SuccessCall()
    {
        ObjectResult apiresult = new ObjectResult(200);

        apiresult = StatusCode(200, "성공!");


        this.NewMessage();

        return apiresult;
    }

    private async void NewMessage()
    {
        
        
        //string sSendModel 
        //    = JsonConvert.SerializeObject(new SignalRSendModel()
        //        {
        //            Sender = "Server"
        //            , To = ""
        //            , Command = "MsgSend"
        //            , Message = "메시지닷!"
        //        });

        //await this._hubContext
        //    .Clients
        //    .All
        //    .SendAsync("ReceiveMessage", sSendModel);

        await this._ChatHubContext.SendUser_All(new SignalRSendModel()
                {
                    Sender = "Server"
                    , To = ""
                    , Command = "MsgSend"
                    , Message = "메시지닷!"
                });
    }


}
