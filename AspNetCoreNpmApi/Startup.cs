using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace AspNetCoreNpmApi;

/// <summary>
/// https://romansimuta.com/posts/how-to-add-webpack-4-to-asp-net-core-3-1-mvc-application-step-by-step/
/// </summary>
public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        //API���� �Ľ�Į ���̽� �����ϱ�
        services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            //������ ���
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }

        //������ ���
        app.UseSwagger();
        app.UseSwaggerUI();

        //3.0 api ���Ʈ
        app.UseRouting();

        //�⺻ ������
        app.UseDefaultFiles();
        //wwwroot
        app.UseStaticFiles();


        //3.0 api ���Ʈ ����
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
