namespace JwtAuth;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

/// <summary>
/// ���� �ʼ� �Ӽ�
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// ������û�� �Դ�.
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
        {//AllowAnonymous���� �����Ǿ� �ִ�.

            //������ ��ŵ�Ѵ�.
            return;
        }
            

        //�������� Ȯ��
        long nUser = 0;
        foreach (Claim item in context.HttpContext.User.Claims.ToArray())
        {
            if (item.Type == "idUser")
            {//���� ������ �ִ�.
                nUser = Convert.ToInt64(item.Value);
                break;
            }
        }

        if (0 >= nUser)
        {//���������� ����.
            
            //401����
            context.Result 
                = new JsonResult(new { message = "Unauthorized" }) 
                        { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}