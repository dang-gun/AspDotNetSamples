using AspNetCoreNpmApi.Global;
using AspNetCoreNpmApi.Models;
using Microsoft.AspNetCore.Mvc;
using ModelsDB;

namespace AspNetCore6DefultSetting.Controllers
{
	/// <summary>
	/// 사인 관련(인,아웃,조인)
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SignController : ControllerBase
	{
		[HttpPut]
		public ActionResult<SignInModel> SignIn(
					[FromForm] string sSignName
					, [FromForm] string sPassword)
		{
			//로그인 처리용 모델
			SignInModel smResult = new SignInModel();

			User? findUser 
				= GlobalStatic.Users.Where(w=>
						w.SignName == sSignName
						&& w.PasswordHash == sPassword)
				.FirstOrDefault();

			if (null != findUser)
			{//유저 찾음
				smResult.Complete = true;
				smResult.Token
					= String.Format("{0}▩{1}"
						, sSignName
						, Guid.NewGuid().ToString());
			}


			return smResult;
		}

		/// <summary>
		/// 지정한 유저의 정보를 준다.
		/// </summary>
		/// <param name="idUser"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<SignInfoModel> SignInfo(long idUser)
		{
			//로그인 처리용 모델
			SignInfoModel smResult = new SignInfoModel();

			User? findUser
				= GlobalStatic.Users.Where(w =>
						w.idUser == idUser)
				.FirstOrDefault();

			if (null != findUser)
			{//유저 찾음
				smResult.Complete = true;
				smResult.UserInfo = findUser;
			}


			return smResult;
		}
	}
}
