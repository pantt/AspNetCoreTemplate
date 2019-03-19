using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Autofac;
using AutoMapper;
using iCTR.TB.Framework.Configuration;
using iCTR.TB.Framework.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetCoreFrame.Application.AutoMapper;
using NetCoreFrame.Common.Authentication;
using NetCoreFrame.Entities;
using NetCoreFrame.WebApi;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
//using NetCoreFrame.Repository.Dapper;
using NetCoreFrame.Repository.EF;
using iCTR.TB.Framework.Core.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Microsoft.Extensions.Options;

//using NetCoreFrame.Repository.Dapper;

namespace NetCoreFrame
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration">IHostingEnvironment</param>
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _env = env;
            AppSettings = configuration.GetSection("AppSettings");
        }
        /// <summary>  
        /// IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// appsetiong.json配置
        /// </summary>
        public IConfigurationSection AppSettings { get; }

        private readonly IHostingEnvironment _env;
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services">services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            //注入全局异常配置项
            GlobalExceptionMessage globalOptions = new GlobalExceptionMessage();
            Configuration.GetSection(nameof(GlobalExceptionMessage)).Bind(globalOptions);
            
            #region 数据库配置

            var connection = Configuration.GetConnectionString("DefaultConnection");
            //工作单元注入，每次请求创建一个实例，避免多个请求争抢同一context
            services.AddScoped<IUnitOfWork>(d => new UnitOfWork(connection));


            #endregion

            

            #region 权限相关

            //jwt
            var jwtOptions = new JwtOptions();
            Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
            //注入JWT授权配置项
            services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));
            var keyByteArray = Encoding.ASCII.GetBytes(jwtOptions.SymmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            //增加授权
            services.AddAuthorization(auth =>
            {
                //配置授权方式
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });
            //授权参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    //使用https
                    //o.RequireHttpsMetadata = true;
                    o.TokenValidationParameters = tokenValidationParameters;
                });
            services.AddScoped<AuthFactory>();

            #endregion

            #region 注入http请求

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(
                p => p.GetService<IHttpContextAccessor>().HttpContext.User
            );
            services.AddScoped<IUserContext, UserContext>();

            #endregion

            //跨域配置，项目部署时若以虚拟目录方式创建站点，删除该配置        
            services.AddCors(options =>
                options.AddPolicy(
                    "AllowAllMethods",
                    builder => builder.AllowAnyOrigin() /*.WithOrigins(urls)*/
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition")
                ));
            //全局过滤器
            services.AddMvc(options =>
            {
                //全局异常捕获
                options.Filters.Add(new HttpGlobalExceptionFilter(_env, globalOptions, new ExceptionProcess()));
                //请求返回结果捕获
                options.Filters.Add<HttpResultFilter>();
            });

            #region AutoMapper
            //automapper注册映射配置
            var mappConfiger = AutoMapperConfig.RegisterMappings();
            services.AddAutoMapper(cfg => mappConfiger.CreateMapper());
            services.AddSingleton<IMapper, Mapper>();

            #endregion

            #region swagger
            //swagger生成调试
            if (Convert.ToBoolean(AppSettings["SwaggerEnable"]))
            {
                services.AddSwaggerGen(c =>
                           {
                               c.SwaggerDoc("BaseAPI", new Info { Title = "BaseAPI" });
                               c.OrderActionsBy((apiDesc) => $"{apiDesc.HttpMethod}");
                               c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                               {
                                   Description =
                                       "JWT 授权设置: \"Authorization: Bearer {token}\"",
                                   Name = "Authorization",
                                   In = "header",
                                   Type = "apiKey"
                               });
                               c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "Bearer", Enumerable.Empty<string>() }});
                var app = PlatformServices.Default.Application;
               var xml= Path.Combine(app.ApplicationBasePath, "NetCoreFrame.WebApi.xml");
                c.IncludeXmlComments(xml);

                           });
            }
            #endregion
        }

        /// <summary>
        /// ConfigureContainer
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">host</param>
        /// <param name="loggerFactory">logger</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region 权限相关

            app.UseAuthentication();

            #endregion

            #region 日志配置

            loggerFactory.AddSerilog();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("Logs/ts-{Date}.txt");

            #endregion

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            if (Convert.ToBoolean(AppSettings["SwaggerEnable"]))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint(AppSettings["SwaggerJsonPath"], "BaseAPI"); });
            }
            app.UseMvc();
        }
    }
}