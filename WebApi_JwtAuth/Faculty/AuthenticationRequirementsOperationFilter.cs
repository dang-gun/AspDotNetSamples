using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAssist
{
	/// <summary>
	/// https://github.com/domaindrivendev/Swashbuckle.AspNetCore
	/// </summary>
	public class AuthenticationRequirementsOperationFilter : IOperationFilter
	{
		/// <summary>
		/// 스웨거용 bearer 체크 필터
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="context"></param>
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation.Security == null)
				operation.Security = new List<OpenApiSecurityRequirement>();


			var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
			operation.Security.Add(new OpenApiSecurityRequirement
			{
				[scheme] = new List<string>()
			});
		}
	}
}
