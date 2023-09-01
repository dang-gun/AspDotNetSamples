using SignalRWebpack.Hubs;

namespace SignalRWebpack;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //�ñ׳�R ����
        //.AddControllers���� �տ� �;� �Ѵ�.
        builder.Services.AddSignalR();
        //�ñ׳�R ���� CORS ����
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

        // MapHub ���� �տ� �;� �Ѵ�.
        app.UseCors();
        //MapControllers ���� �տ� �;� �Ѵ�.
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