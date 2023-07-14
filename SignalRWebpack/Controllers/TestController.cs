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


        return apiresult;
    }

    /// <summary>
    /// 전체에 메시지를 보낸다.
    /// </summary>
    /// <param name="sMessage"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult MessageAll(string sMessage)
    {
        ObjectResult apiresult = new ObjectResult(200);

        apiresult = StatusCode(200, "성공!");

        this.NewMessage(string.Empty, sMessage);

        return apiresult;
    }

    /// <summary>
    /// 지정된 유저에게 메시지를 보낸다.
    /// </summary>
    /// <param name="sTo"></param>
    /// <param name="sMessage"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult MessageTo(string sTo, string sMessage)
    {
        ObjectResult apiresult = new ObjectResult(200);

        apiresult = StatusCode(200, "성공!");

        this.NewMessage(sTo, sMessage);

        return apiresult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sTo"></param>
    /// <param name="sMessage"></param>
    private async void NewMessage(string sTo, string sMessage)
    {
        if(string.Empty == sTo)
        {
            await this._ChatHubContext.SendUser_All(
                new SignalRSendModel()
                {
                    Sender = "Server"
                    , To = ""
                    , Command = "MsgSend"
                    , Message = sMessage
                });
        }
        else
        {
            await this._ChatHubContext.SendUser_Name(
                sTo
                , new SignalRSendModel()
                {
                    Sender = "Server"
                    , To = ""
                    , Command = "MsgSend"
                    , Message = sMessage
                });
        }

        
    }


}
