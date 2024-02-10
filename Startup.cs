using RepoDb;
using Locum_Backend;
using Locum_Backend.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Locum_Backend.Repositories;
using System.Text;

public class Startup
{




    public void ConfigureServices(IServiceCollection services)
    {
        //string connectionString = "Data Source=10.20.20.104;Initial Catalog=Locum;User Id=sa;Password=YourPassword;trustServerCertificate=true;TrustServerCertificate=Yes";

        //services.AddScoped<IUserRepository, UserRepository>(); // Register your UserRepository
        services.AddRazorPages();
        services.AddSignalR();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowCredentials());
        });

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton<UserRepository>();



        // Other services configuration...
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("/notificationHub");
            endpoints.MapHub<LocumApprovalHub>("/Hubs");
        });
    }
}
