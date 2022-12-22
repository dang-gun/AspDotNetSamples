using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
else
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


//3.0 api ���Ʈ
app.UseRouting();
//https�� �ڵ� ���𷺼�
app.UseHttpsRedirection();

//�⺻ ������
app.UseDefaultFiles();
//wwwroot
app.UseStaticFiles();


app.MapControllers();

app.Run();
#endregion
