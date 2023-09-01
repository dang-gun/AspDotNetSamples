using SignalRWebpack.Hubs;

namespace SignalRWebpack;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //시그널R 설정
        //.AddControllers보다 앞에 와야 한다.
        builder.Services.AddSignalR();
        //시그널R 설정 CORS 설정
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:9500")
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST")
                        .AllowCredentials();
                });
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

        //app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        // MapHub 보다 앞에 와야 한다.
        app.UseCors();
        //MapControllers 보다 앞에 와야 한다.
        app.MapHub<ChatHub>("/chatHub");
        //
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapHub<ChatHub>("/chatHub");
        //});

        app.MapControllers();

        app.Run();
    }
}