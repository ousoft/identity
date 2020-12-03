using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Oyang.Identity.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Oyang.Identity.Infrastructure.Common;

namespace Oyang.Identity.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IAppSettings appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddMvcOptions(t => t.Filters.Add(new Filters.WebApiResponseExceptionFilter()));

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidIssuer = appSettings.Jwt.Issuer,
                         ValidAudience = appSettings.Jwt.Audience,
                         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettings.Jwt.SecurityKey)),
                         ClockSkew = TimeSpan.Zero,
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateIssuerSigningKey = true,
                         ValidateLifetime = true,

                         /***********************************TokenValidationParameters的参数默认值***********************************/
                         // RequireSignedTokens = true,
                         // SaveSigninToken = false,
                         // ValidateActor = false,
                         // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                         // ValidateAudience = true,
                         // ValidateIssuer = true, 
                         // ValidateIssuerSigningKey = false,
                         // 是否要求Token的Claims中必须包含Expires
                         // RequireExpirationTime = true,
                         // 允许的服务器时间偏移量
                         // ClockSkew = TimeSpan.FromSeconds(300),
                         // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                         // ValidateLifetime = true
                     };
                 });
            //services.AddCors(options =>
            //{
            //    var originsString = Configuration.GetValue<string>("Origins");
            //    var origins = originsString.Split(',');
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
            //        });
            //});
            services.AddSingleton<IMapper>(t =>
            {
                var mapperConfiguration = new MapperConfiguration(t =>
                {
                    t.AddProfile<IdentityProfile>();
                });
                return mapperConfiguration.CreateMapper();
            });

            services.AddOyangIdentity();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
