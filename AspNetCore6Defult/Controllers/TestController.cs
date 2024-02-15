using Microsoft.AspNetCore.Mvc;

namespace AspNetCore6Defult.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TestController : ControllerBase
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
    }
}
