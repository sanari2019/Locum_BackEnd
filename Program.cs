using RepoDb;
using Locum_Backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Locum_Backend.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Locum_Backend.Models;
using Locum_Backend.Controllers;
using Locum_Backend.Repositories;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig); // Provide a default configuration if null

// builder.Services.AddSingleton<EmailConfiguration>(emailConfig);

builder.Services.AddScoped<IEmailSender, EmailSender>();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration
 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
builder.Services.Configure<Setting>(builder.Configuration.GetSection("Setting"));
var setting = builder.Configuration.GetSection("Setting").Get<Setting>();
#pragma warning disable CS8604 // Possible null reference argument.
builder.Services.AddSingleton<Setting>(setting);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS0612 // Type or member is obsolete
setting.InitializeRepoDb();
#pragma warning restore CS0612 // Type or member is obsolete
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<EmployeeTypeRepository>();
builder.Services.AddSingleton<DepartmentRepository>();
builder.Services.AddSingleton<ShiftRepository>();
builder.Services.AddSingleton<ApprovalRequestRepository>();
builder.Services.AddSingleton<ApprovalRepository>();
builder.Services.AddScoped<UsersRolesRepository>();
builder.Services.AddScoped<WardSupervisorRepository>();
builder.Services.AddScoped<WardRepository>();
builder.Services.AddScoped<PatientRepository>();
builder.Services.AddScoped<ValidatedPatientRepository>();
builder.Services.AddScoped<RequestFormPatientRepository>();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("CorsPolicy");


app.UseAuthorization();

app.MapRazorPages();


app.MapRazorPages();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<LocumApprovalHub>("/Hubs");

app.Run();
