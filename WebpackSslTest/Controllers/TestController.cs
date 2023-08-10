using Microsoft.AspNetCore.Mvc;

namespace AlAdmin.Controllers;

/// <summary>
/// 테스트용 API
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : Controller
{
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
    /// 데이터 입력 테스트
    /// </summary>
    /// <param name="nData1"></param>
    /// <param name="sData2"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<string> DataTest(
        [FromForm] int nData1
        , [FromForm] string sData2)
    {
        string sReturn
            = string.Format("Data1 : {0}, Data2 : {1}"
                            , nData1, sData2);

        return sReturn;
    }

    /// <summary>
    /// 에러 테스트
    /// </summary>
    /// <param name="nType"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult ErrorCall([FromForm] int nType)
    {
        ObjectResult apiresult = new ObjectResult(200);

        if (0 == nType)
        {
            apiresult = StatusCode(200, "성공!");
        }
        else
        {
            apiresult = StatusCode(500, "에러!");
        }


        return apiresult;
    }
}
