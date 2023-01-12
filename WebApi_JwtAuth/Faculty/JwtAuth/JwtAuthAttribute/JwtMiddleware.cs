namespace JwtAuth;

using JwtAuth;
using JwtAuth.Models;
using Microsoft.Extensions.Options;
using ModelsDB;
using System.Security.Claims;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtAuthSettingModel _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtAuthSettingModel> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(
        HttpContext context
        , IJwtUtils jwtUtils)
    {
        //토큰 추출
        string? token 
            = context.Request
                .Headers["Authorization"]
                .FirstOrDefault()?
                .Split(_appSettings.AuthTokenStartName_Complete)
                .Last();

        if (null != token)
        {//토큰이 있다.

            //토큰에서 idUser 추출
            int? idUser = jwtUtils.AccessTokenValidate(token);


            if (idUser != null && 0 < idUser)
            {//추출된 아이디가 있다.

                //엑세스 토큰에 데이터가 있으면 클레임데이터를 추가해 준다.
                var claims = new List<Claim>
                {
                    new Claim("idUser", idUser.ToString()!)
                };

                //HttpContext에 클래임 정보를 넣어준다.
                ClaimsIdentity appIdentity = new ClaimsIdentity(claims);
                context.User.AddIdentity(appIdentity);
            }
        }

        await _next(context);
    }
}