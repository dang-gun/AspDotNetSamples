using AspNetCore6DefultSetting_React.Global;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


#region Startup
//appsettings.json
IConfiguration configuration = builder.Configuration;

//DB Ŀ���� ��Ʈ�� �޾ƿ���
string sConnectStringSelect = "Test_sqlite";
GlobalStatic.DBType = configuration[sConnectStringSelect + ":DBType"].ToLower();
GlobalStatic.DBString = configuration[sConnectStringSelect + ":ConnectionString"];
#endregion

#region ConfigureServices
//API���� �Ľ�Į ���̽� �����ϱ�
builder.Services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion


#region Configure
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{//���� ���������� ������ ���
	app.UseSwagger();
	app.UseSwaggerUI();
}

//3.0 api ���Ʈ
app.UseRouting();
//https�� �ڵ� ���𷺼�
app.UseHttpsRedirection();

//�⺻ ������
app.UseDefaultFiles();
//�� ��Ʈ
//app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
	app.UseStaticFiles(new StaticFileOptions
	{
		FileProvider = new PhysicalFileProvider(
	   Path.Combine(builder.Environment.ContentRootPath, @"wwwroot\development")),
	});
}
else
{
	app.UseStaticFiles(new StaticFileOptions
	{
		FileProvider = new PhysicalFileProvider(
	   Path.Combine(builder.Environment.ContentRootPath, @"wwwroot\production")),
	});
}



app.MapControllers();

app.Run();
#endregion
