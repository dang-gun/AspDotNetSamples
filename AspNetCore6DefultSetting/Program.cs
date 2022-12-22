using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureServices
//API모델을 파스칼 케이스 유지하기
builder.Services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion


#region Configure
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{//개발 버전에서만 스웨거 사용
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


//3.0 api 라우트
app.UseRouting();
//https로 자동 리디렉션
app.UseHttpsRedirection();

//기본 페이지
app.UseDefaultFiles();
//wwwroot
app.UseStaticFiles();


app.MapControllers();

app.Run();
#endregion
