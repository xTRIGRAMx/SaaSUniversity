
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Data;
using SaaSUniversity.Server.Services;
using System;
using System.Text;

namespace SaaSUniversity.Server
{
    public class Program
    {

        public static void Main(string[] args)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWasm",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:7002")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); 
                    });
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".AspNetCore.Identity.Application";
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CourseEnrollmentDb"));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // Configure the HTTP request pipeline.

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData.Initialize(db);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowWasm");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
