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
    /// ������ ��ū ����
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public string AccessTokenGenerate(int idUser);

    /// <summary>
    /// ������ ��ū Ȯ��.
    /// </summary>
    /// <remarks>�̵������� ȣ���ؼ� ����Ѵ�.</remarks>
    /// <param name="token"></param>
    /// <returns>ã�Ƴ� idUser</returns>
    public int? AccessTokenValidate(string token);

    /// <summary>
    /// ���÷��� ��ū ����.
    /// </summary>
    /// <remarks>�ߺ��˻�� ���� �����Ƿ� �ʿ��ϴٸ� ȣ�����ʿ��� �ߺ��˻縦 �ؾ� �Ѵ�.</remarks>
    /// <returns></returns>
    public string RefreshTokenGenerate();

    /// <summary>
    /// HttpContext.User�� Ŭ������ �˻��Ͽ� ���� ���������� �޴´�.
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
    /// ������ ���� ����
    /// </summary>
    private readonly JwtAuthSettingModel _JwtAuthSetting;

    public JwtUtils(IOptions<JwtAuthSettingModel> appSettings)
    {
        //���� ������ �ޱ�
        _JwtAuthSetting = appSettings.Value;

        if (_JwtAuthSetting.Secret == null 
            || _JwtAuthSetting.Secret == string.Empty)
        {//��ũ�� ���� ����.

            //���� �����Ѵ�.
            _JwtAuthSetting.Secret 
                = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }

    
    public string AccessTokenGenerate(int idUser)
    {
        //��ū �ڵ鷯
        JwtSecurityTokenHandler tokenHandler 
            = new JwtSecurityTokenHandler();
        //��ũ�� Ű ��ȯ
        byte[] key = Encoding.ASCII.GetBytes(_JwtAuthSetting.Secret!);
        //��ū ����
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("idUser", idUser.ToString()) }),
            //15�� ����
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        //��ū ����
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? AccessTokenValidate(string token)
    {
        if (token == null)
        {
            return null;
        }

        //��ū �ڵ鷯
        JwtSecurityTokenHandler tokenHandler 
            = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_JwtAuthSetting.Secret!);
        try
        {
            //��ū ��ȿ�� �˻� ����
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

            //��� �ؼ�
            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            int accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "idUser").Value);
            return accountId;
        }
        catch
        {
            //ó���߿� ������ ���� id�� ã�� ���ߴٴ� �ǹ��̹Ƿ�
            //null�� ���Ͻ�Ų��.
            return null;
        }
    }

    public string RefreshTokenGenerate()
    {
        //�����ϰ� 64�ڸ� ����
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }


    public long? ClaimDataGet(ClaimsPrincipal claimsPrincipal)
    {
        //�������� Ȯ��
        long nUser = 0;
        foreach (Claim item in claimsPrincipal.Claims.ToArray())
        {
            if (item.Type == "idUser")
            {//���� ������ �ִ�.
                nUser = Convert.ToInt64(item.Value);
                break;
            }
        }

        return nUser;
    }
}