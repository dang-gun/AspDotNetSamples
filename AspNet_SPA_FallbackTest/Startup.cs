using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;

namespace AspNet_SPA_FallbackTest;

public class Startup
{
	/// <summary>
	/// 
	/// </summary>
	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="app"></param>
	/// <param name="env"></param>
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{

		// Configure the HTTP request pipeline.
		if (env.IsDevelopment())
		{//개발 버전에서만 스웨거 사용
			app.UseSwagger();
			app.UseSwaggerUI();
		}


		//3.0 api 라우트
		app.UseRouting();
		//https로 자동 리디렉션
		app.UseHttpsRedirection();



		//기본 페이지
		app.UseDefaultFiles();
		app.UseStaticFiles();



		app.UseEndpoints(endpoints =>
		{
			
			endpoints.MapControllers();
			endpoints.MapFallbackToFile("/index.html");
		});
	}
}
