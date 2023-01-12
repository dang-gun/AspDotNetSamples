using JwtAuth;
using JwtAuth.Models;
using WebApi_JwtAuthTest.Global;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using SwaggerAssist;

namespace WebApi_JwtAuthTest
{
	public class Startup
	{
        /// <summary>
        /// 전달받은 컨피그(appsettings.json)
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            //전달받은 'appsettings.json'백업
            Configuration = configuration;

            //DB 커낵션 스트링 받아오기
            string sConnectStringSelect = "Test_sqlite";
            GlobalStatic.DBType = Configuration[sConnectStringSelect + ":DBType"].ToLower();
            GlobalStatic.DBString = Configuration[sConnectStringSelect + ":ConnectionString"];

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //API모델을 파스칼 케이스 유지하기
            services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });


            //Jwt Auth Setting 정보 전달
            //Configuration["JwtSecretSetting:Secret"] = "";
            services.Configure<JwtAuthSettingModel>(Configuration.GetSection("JwtSecretSetting"));
            services.AddScoped<IJwtUtils, JwtUtils>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            //스웨거 문서정보를 생성 한다.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "SPA NetCore Foundation API",
                        Description = "[ASP.NET Core] .NET Core로 구현한 SPA(Single Page Applications)(5) - 스웨거(Swagger) 설정 <br /> https://blog.danggun.net/7689",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Dang-Gun Roleeyas",
                            Email = string.Empty,
                            Url = new Uri("https://blog.danggun.net/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                //인증UI **************************************
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "로그인 후 전달받은 '엑세스 토큰(access token)'을 헤더의'Authorization'에 'Bearer access token' 형태로 담아 전달해야 합니다.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                //인증 필터
                c.OperationFilter<AuthenticationRequirementsOperationFilter>();
                //주석 표시기능
                //c.IncludeXmlComments(string.Format(@"{0}\SPA_NetCore_Foundation06.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                //c.IncludeXmlComments(@"WebApi_JwtAuth.xml");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //스웨거 사용
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            //JwtAuth 미들웨어 주입
            app.UseMiddleware<JwtMiddleware>();

            //스웨거 사용
            app.UseSwagger();
            app.UseSwaggerUI();

            //3.0 api 라우트
            app.UseRouting();

            //기본 페이지
            app.UseDefaultFiles();
            //wwwroot
            app.UseStaticFiles();


            //3.0 api 라우트 끝점
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
