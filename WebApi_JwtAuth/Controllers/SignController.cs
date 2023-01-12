using WebApi_JwtAuthTest.Global;
using WebApi_JwtAuthTest.Models;
using Microsoft.AspNetCore.Mvc;
using ModelsDB;
using JwtAuth;
using System.Net;

namespace WebApi_JwtAuthTest.Controllers;

/// <summary>
/// 사인 관련(인,아웃,조인)
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class SignController : ControllerBase
{
	public readonly string AccessTokenName = "AccessToken";
	public readonly string RefreshTokenName = "RefreshToken";


	/// <summary>
	/// 인증 유틸
	/// </summary>
	private readonly IJwtUtils _JwtUtils;

	/// <summary>
	/// 생성자
	/// </summary>
	/// <param name="jwtUtils"></param>
	public SignController(IJwtUtils jwtUtils)
	{
		this._JwtUtils = jwtUtils;
	}

	/// <summary>
	/// 가입
	/// </summary>
	/// <param name="sSignName"></param>
	/// <param name="sPassword"></param>
	/// <returns></returns>
	[HttpPost]
	public ActionResult<ApiResultModel> Register(
		[FromForm] string sSignName
		, [FromForm] string sPassword)
	{
		//로그인 처리용 모델
		ApiResultModel arReturn = new ApiResultModel();
		arReturn.Complete = true;

		if (string.Empty == sSignName)
		{
			arReturn.Complete = false;
			arReturn.Message = "사인 인 이름은 필수 입니다.";
		}
		else if (string.Empty == sPassword)
		{
			arReturn.Complete = false;
			arReturn.Message = "사인 인 비밀번호는 필수 입니다.";
		}

		if (true == arReturn.Complete)
		{
			using (ModelsDbContext db1 = new ModelsDbContext())
			{
				//같은 이름이 있는지 찾는다.
				User? findUser
					= db1.User.Where(w => w.SignName == sSignName)
						.FirstOrDefault();

				if (null != findUser)
				{
					arReturn.Complete = false;
					arReturn.Message = "이미 있는 사인 인 이름입니다.";
				}


				if (true == arReturn.Complete)
				{
					User newUser = new User();
					newUser.SignName = sSignName;
					newUser.PasswordHash = sPassword;
					db1.User.Add(newUser);
				}

				db1.SaveChanges();
			}//end using db1
		}

		return arReturn;
	}

	/// <summary>
	/// 사인 인
	/// </summary>
	/// <param name="sSignName"></param>
	/// <param name="sPassword"></param>
	/// <returns></returns>
	[HttpPut]
	[AllowAnonymous]
	public ActionResult<SignInModel> SignIn(
				[FromForm] string sSignName
				, [FromForm] string sPassword)
	{
		//로그인 처리용 모델
		SignInModel arReturn = new SignInModel();

		DateTime dtNow = DateTime.Now;

		using (ModelsDbContext db1 = new ModelsDbContext())
		{
			User? findUser
			= db1.User.Where(w =>
					w.SignName == sSignName
					&& w.PasswordHash == sPassword)
			.FirstOrDefault();

			if (null != findUser)
			{//유저 찾음

				arReturn.idUser = findUser.idUser;
				arReturn.Complete = true;

				//엑세스 토큰 생성
				string at = this._JwtUtils.AccessTokenGenerate(findUser.idUser);
				string st = this._JwtUtils.RefreshTokenGenerate();

				while (true)
				{
					if (true == db1.UserRefreshToken.Any(a => a.RefreshToken == st))
					{//같은 토큰이 있다.
						st = this._JwtUtils.RefreshTokenGenerate();
					}
					else
					{
						//새로운 값이면 완료
						break;
					}
				}

				//기존 토큰 만료 처리
				IQueryable<UserRefreshToken> iqFindURT
					= db1.UserRefreshToken
						.Where(w => w.idUser == findUser.idUser
								&& true == w.ActiveIs);
				//linq는 데이터를 수정할때는 좋은 솔류션이 아니다.
				//반복문으로 직접수정하는 것이 훨씬 성능에 도움이 된다.
				foreach (UserRefreshToken itemURT in iqFindURT)
				{
					//만료 시간을 기입함
					itemURT.RevokeTime = dtNow;
					itemURT.ActiveCheck();
				}


				//새로운 토큰 생성
				arReturn.AccessToken = at;
				arReturn.RefreshToken = st;

				UserRefreshToken newURT = new UserRefreshToken()
				{
					idUser = findUser.idUser,
					RefreshToken = st,
					ExpiresTime = DateTime.UtcNow.AddSeconds(1296000),
				};
				newURT.ActiveCheck();


				db1.UserRefreshToken.Add(newURT);
				db1.SaveChanges();

				//쿠키에 저장요청
				this.Cookie_AccessToken(at);
				this.Cookie_RefreshToken(st);
			}
		}//end using db1

		return arReturn;
	}

	/// <summary>
	/// 사인 아웃
	/// </summary>
	/// <remarks>
	/// ControllerBase.SignOut가 있어서 new로 선언한다<br />
	/// ControllerBase.SignOut은 표준화된 외부 로그인방식 같은데....
	/// 어떻게 활용할지는 연구를 해봐야 할듯하다.
	/// </remarks>
	/// <returns></returns>
	[HttpPut]
	[Authorize]
	public new ActionResult<ApiResultModel> SignOut()
	{
		ApiResultModel arReturn = new ApiResultModel();
		arReturn.Complete = true;

		DateTime dtNow = DateTime.Now;

		//대상 유저를 검색하고
		long? idUser = this._JwtUtils.ClaimDataGet(HttpContext.User);
		if (null != idUser)
		{//대상이 있다.
			using (ModelsDbContext db1 = new ModelsDbContext())
			{
				//가지고 있는 기존 리플레시 토큰 만료 처리
				IQueryable<UserRefreshToken> iqFindURT
					= db1.UserRefreshToken
						.Where(w => w.idUser == idUser
								&& true == w.ActiveIs);
				//linq는 데이터를 수정할때는 좋은 솔류션이 아니다.
				//반복문으로 직접수정하는 것이 훨씬 성능에 도움이 된다.
				foreach (UserRefreshToken itemURT in iqFindURT)
				{
					//만료 시간을 기입함
					itemURT.RevokeTime = dtNow;
					itemURT.ActiveCheck();
				}

				db1.SaveChanges();
			}//end using db1

			//쿠키에 저장요청
			//빈값을 저장해서 기존 토큰을 제거요청한다.
			this.Cookie_AccessToken("");
			this.Cookie_RefreshToken("");
		}

		return arReturn;
	}

	/// <summary>
	/// 지정한 유저의 정보를 준다.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	//[Authorize]
	public ActionResult<SignInfoModel> SignInfo()
	{
		//로그인 처리용 모델
		SignInfoModel arReturn = new SignInfoModel();

		long? idUser = this._JwtUtils.ClaimDataGet(HttpContext.User);

		using (ModelsDbContext db1 = new ModelsDbContext())
		{
			User? findUser
			= db1.User.Where(w =>
					w.idUser == idUser)
			.FirstOrDefault();

			if (null != findUser)
			{//유저 찾음
				arReturn.Complete = true;
				arReturn.UserInfo = findUser;
			}
			else
			{
				arReturn.Complete = false;
			}

		}//end using db1


		return arReturn;
	}

	/// <summary>
	/// 리플레시 토큰으로 새로운 액세스 토큰을 생성해준다.
	/// </summary>
	/// <returns></returns>
	[HttpPut]
	public ActionResult<SignInModel> RefreshToken()
	{
		SignInModel arReturn = new SignInModel();
		arReturn.Complete = true;

		DateTime dtNow = DateTime.Now;

		//쿠키에서 리플레시토큰을 읽는다.
		string? sST = Request.Cookies[RefreshTokenName];

		string sNewST = string.Empty;
		string sNewAT = string.Empty;

		if (null != sST && string.Empty != sST)
		{
			using (ModelsDbContext db1 = new ModelsDbContext())
			{
				//리플레시 토큰 검색
				UserRefreshToken? findURT
					= db1.UserRefreshToken
						.Where(w => w.RefreshToken == sST)
						.FirstOrDefault();

				if (null != findURT
					&& true == findURT.ActiveCheck().ActiveIs)
				{//리플레시 토큰 사용가능

					User? findUser
						= db1.User
							.Where(w => w.idUser == findURT.idUser)
							.FirstOrDefault();
					if (null != findUser)
					{

						//옵션에 따라 리플레시토큰을 재갱신 해야 한다.
						sNewST = sST;
						sNewAT = this._JwtUtils.AccessTokenGenerate(findUser.idUser);

						arReturn.AccessToken = sNewAT;
						arReturn.RefreshToken = sNewST;
					}
					else
					{

						//없으면 권한 없음 에러를 낸다.
						arReturn.Complete = false;
						Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					}
				}
				else
				{

					//없으면 권한 없음 에러를 낸다.
					arReturn.Complete = false;
					Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				}

				db1.SaveChanges();

			}//end using db1
		}
		else
		{//리플레시 토큰이 없다.

			//없으면 권한 없음 에러를 낸다.
			arReturn.Complete = false;
			Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		}

		//쿠키에 새로운 토큰 저장
		this.Cookie_AccessToken(sNewAT);
		this.Cookie_RefreshToken(sNewST);

		return arReturn;
	}


	/// <summary>
	/// 가지고 있는 리플레시 토큰을 만료 시킨다.
	/// </summary>
	/// <returns></returns>
	[HttpDelete]
	public ActionResult<ApiResultModel> RefreshTokenRevoke()
	{
		ApiResultModel arReturn = new ApiResultModel();
		arReturn.Complete = true;

		DateTime dtNow = DateTime.Now;


		//쿠키에서 토큰을 읽는다.
		string? sST = Request.Cookies[RefreshTokenName];

		if (null != sST && string.Empty != sST)
		{//비어있지 않다.

			//제거작업
			using (ModelsDbContext db1 = new ModelsDbContext())
			{
				//리플레시 토큰 검색
				List<UserRefreshToken> findURT
					= db1.UserRefreshToken
						.Where(w => w.RefreshToken == sST)
						.ToList();

				//검색된 리플레시 토큰을 만료 시킨다.
				foreach (UserRefreshToken itemURT in findURT)
				{
					itemURT.RevokeTime = dtNow;
					itemURT.ActiveCheck();
				}

				db1.SaveChanges();
			}//end using db1
		}


		//쿠키에서 토큰 제거
		this.Cookie_AccessToken("");
		this.Cookie_RefreshToken("");


		return arReturn;
	}

	/// <summary>
	/// 로그인된 계정이 가지고 있는 모든 리플레시 토큰을 만료 시킨다.
	/// </summary>
	/// <returns></returns>
	[HttpDelete]
	[Authorize]
	public ActionResult<ApiResultModel> RefreshTokenRevokeAll()
	{
		//로그인 처리용 모델
		ApiResultModel arReturn = new ApiResultModel();
		arReturn.Complete = true;

		DateTime dtNow = DateTime.Now;


		//대상 유저를 검색한다.
		long? idUser = this._JwtUtils.ClaimDataGet(HttpContext.User);

		if (null != idUser)
		{//비어있지 않다.

			//제거작업
			using (ModelsDbContext db1 = new ModelsDbContext())
			{
				//리플레시 토큰 검색
				List<UserRefreshToken> findURT
					= db1.UserRefreshToken
						.Where(w => w.idUser == idUser
								&& true == w.ActiveIs)
						.ToList();

				//검색된 리플레시 토큰을 만료 시킨다.
				foreach (UserRefreshToken itemURT in findURT)
				{
					itemURT.RevokeTime = dtNow;
					itemURT.ActiveCheck();
				}

				db1.SaveChanges();
			}//end using db1
		}
		else
		{
			arReturn.Complete = false;
			Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		}


		if (true == arReturn.Complete)
		{
			//쿠키에서 토큰 제거
			this.Cookie_AccessToken("");
			this.Cookie_RefreshToken("");
		}
			

		return arReturn;
	}


	/// <summary>
	/// 쿠키에 엑세스 토큰 저장을 요청한다.
	/// </summary>
	/// <param name="token"></param>
	private void Cookie_AccessToken(string token)
	{
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = DateTime.UtcNow.AddDays(7)
		};
		Response.Cookies.Append(AccessTokenName, token, cookieOptions);
	}

	/// <summary>
	/// 쿠기에 리플레이 토큰 저장을 요청한다.
	/// </summary>
	/// <param name="token"></param>
	private void Cookie_RefreshToken(string token)
	{
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = DateTime.UtcNow.AddDays(7)
		};
		Response.Cookies.Append(RefreshTokenName, token, cookieOptions);
	}
}

