

using Microsoft.Extensions.FileProviders;

//var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateBuilder(
	new WebApplicationOptions
	{
		WebRootPath = "wwwroot1"
	});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//기본 페이지
app.UseDefaultFiles();

//웹 루트
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
	   Path.Combine(builder.Environment.ContentRootPath, @"wwwroot1\sub2")),
});





app.MapControllers();

app.Run();
