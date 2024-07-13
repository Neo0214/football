using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using football.Entities;
using football.Repos;
using football.Services;
using football.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Object_Storage;

namespace YourProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        var env = context.HostingEnvironment;
                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                              .AddEnvironmentVariables();
                    });

                    webBuilder.ConfigureServices((context, services) =>
                    {
                        var configuration = context.Configuration;

                        // 从配置中获取 JWT 相关设置
                        var jwtSettings = configuration.GetSection("JwtSettings");
                        var secretKey = jwtSettings.GetValue<string>("SecretKey");
                        var issuer = jwtSettings.GetValue<string>("Issuer");
                        var audience = jwtSettings.GetValue<string>("Audience");

                        // 添加身份验证服务
                        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = issuer,
                                    ValidAudience = audience,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                                };
                            });

                        services.AddControllers();
                        services.AddHttpContextAccessor();
                        services.AddDbContext<FootballContext>(options =>
                            options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21))));
                        // 注册服务
                        services.AddScoped<UserRepo>();
                        services.AddScoped<IUser, UserService>();
                        services.AddScoped<ClubRepo>();
                        services.AddScoped<IClub, ClubService>();
                        services.AddScoped<PlayerRepo>();
                        services.AddScoped<IPlayer, PlayerService>();
                        services.AddScoped<TransferRepo>();
                        services.AddScoped<ITransfer, TransferService>();
                        services.AddScoped<NationRepo>();
                        services.AddScoped<INation, NationService>();
                        services.AddScoped<TrainRepo>();
                        services.AddScoped<ITrain, TrainService>();
                        // OSS 存储服务
                        services.AddSingleton(ObjectStorageContext.Instance);


                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Football API", Version = "v1" });
                        });

                        // 注册 TokenService
                        services.AddSingleton(new Token(secretKey, issuer, audience));

                        // Add CORS policy to allow any origin
                        services.AddCors(options =>
                        {
                            options.AddPolicy("AllowAnyOrigin",
                                builder => builder.AllowAnyOrigin()
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod());
                        });
                    });

                    webBuilder.Configure((context, app) =>
                    {
                        var env = context.HostingEnvironment;

                        if (env.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseHttpsRedirection();

                        app.UseRouting();

                        app.UseCors("AllowAnyOrigin"); // 使用允许任何来源的CORS策略

                        // 启用身份验证中间件
                        app.UseAuthentication();

                        app.UseAuthorization();

                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Football API V1");
                            c.RoutePrefix = string.Empty; // To serve Swagger UI at application's root
                        });
                        // 添加 WebSocket Controller 路由
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}
