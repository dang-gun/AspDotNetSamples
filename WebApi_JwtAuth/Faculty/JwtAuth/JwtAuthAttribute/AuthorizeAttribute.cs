namespace JwtAuth;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

/// <summary>
/// 인증 필수 속성
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// 인증요청이 왔다.
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //
        bool bAllowAnonymous 
            = context.ActionDescriptor
                    .EndpointMetadata
                    .OfType<AllowAnonymousAttribute>().Any();
        if (true == bAllowAnonymous)
        {//AllowAnonymous으로 설정되어 있다.

            //인증을 스킵한다.
            return;
        }
            

        //인증정보 확인
        long nUser = 0;
        foreach (Claim item in context.HttpContext.User.Claims.ToArray())
        {
            if (item.Type == "idUser")
            {//인증 정보가 있다.
                nUser = Convert.ToInt64(item.Value);
                break;
            }
        }

        if (0 >= nUser)
        {//인증정보가 없다.
            
            //401에러
            context.Result 
                = new JsonResult(new { message = "Unauthorized" }) 
                        { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}