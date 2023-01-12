namespace JwtAuth;

using JwtAuth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelsDB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public interface IJwtUtils
{
    /// <summary>
    /// 엑세스 토큰 생성
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public string AccessTokenGenerate(int idUser);

    /// <summary>
    /// 엑세스 토큰 확인.
    /// </summary>
    /// <remarks>미들웨어에서도 호출해서 사용한다.</remarks>
    /// <param name="token"></param>
    /// <returns>찾아낸 idUser</returns>
    public int? AccessTokenValidate(string token);

    /// <summary>
    /// 리플레시 토큰 생성.
    /// </summary>
    /// <remarks>중복검사는 하지 않으므로 필요하다면 호출한쪽에서 중복검사를 해야 한다.</remarks>
    /// <returns></returns>
    public string RefreshTokenGenerate();

    /// <summary>
    /// HttpContext.User의 클레임을 검색하여 유저 고유정보를 받는다.
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <returns></returns>
    public long? ClaimDataGet(ClaimsPrincipal claimsPrincipal);
}

/// <summary>
/// https://jasonwatmore.com/post/2022/01/24/net-6-jwt-authentication-with-refresh-tokens-tutorial-with-example-api#running-angular
/// </summary>
public class JwtUtils : IJwtUtils
{
    /// <summary>
    /// 설정된 세팅 정보
    /// </summary>
    private readonly JwtAuthSettingModel _JwtAuthSetting;

    public JwtUtils(IOptions<JwtAuthSettingModel> appSettings)
    {
        //설정 데이터 받기
        _JwtAuthSetting = appSettings.Value;

        if (_JwtAuthSetting.Secret == null 
            || _JwtAuthSetting.Secret == string.Empty)
        {//시크릿 값이 없다.

            //새로 생성한다.
            _JwtAuthSetting.Secret 
                = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }

    
    public string AccessTokenGenerate(int idUser)
    {
        //토큰 핸들러
        JwtSecurityTokenHandler tokenHandler 
            = new JwtSecurityTokenHandler();
        //시크릿 키 변환
        byte[] key = Encoding.ASCII.GetBytes(_JwtAuthSetting.Secret!);
        //토큰 설정
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("idUser", idUser.ToString()) }),
            //15분 유지
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        //토큰 생성
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? AccessTokenValidate(string token)
    {
        if (token == null)
        {
            return null;
        }

        //토큰 핸들러
        JwtSecurityTokenHandler tokenHandler 
            = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_JwtAuthSetting.Secret!);
        try
        {
            //토큰 유효성 검사 시작
            tokenHandler.ValidateToken(
                token
                , new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }
                , out SecurityToken validatedToken);

            //결과 해석
            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            int accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "idUser").Value);
            return accountId;
        }
        catch
        {
            //처리중에 오류가 나면 id를 찾지 못했다는 의미이므로
            //null을 리턴시킨다.
            return null;
        }
    }

    public string RefreshTokenGenerate()
    {
        //랜덤하게 64자리 생성
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }


    public long? ClaimDataGet(ClaimsPrincipal claimsPrincipal)
    {
        //인증정보 확인
        long nUser = 0;
        foreach (Claim item in claimsPrincipal.Claims.ToArray())
        {
            if (item.Type == "idUser")
            {//인증 정보가 있다.
                nUser = Convert.ToInt64(item.Value);
                break;
            }
        }

        return nUser;
    }
}