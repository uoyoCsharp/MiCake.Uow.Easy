﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace MiCakeDemoApplication.Extensions
{
    public static class AuthenticationServiceCollectionExtension
    {
        /// <summary>
        /// 添加JwtBearer本地验证和微信小程序远程验证方案.
        /// </summary>
        public static AuthenticationBuilder AddWeChatAndJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Authroize.配置微信小程序远程验证方案和JWT Bearer的本地验证验证方案.
            var seurityKey = Encoding.Default.GetBytes(configuration["JwtConfig:SecurityKey"]);
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddWeChatMiniProgram(wechatOptions =>
                     {
                         wechatOptions.WeChatAppId = configuration["WeChatMiniProgram:appid"];
                         wechatOptions.WeChatSecret = configuration["WeChatMiniProgram:secret"];

                         wechatOptions.SaveSessionKeyToCache = true;
                         wechatOptions.CustomerLoginState += context =>
                         {
                             var currentUrl = $"WeChatUser/Login?key={context.SessionInfoKey}";
                             context.HttpContext.Response.Redirect(currentUrl);

                             return Task.CompletedTask;
                         };
                     })
                     .AddJwtBearer(jwtOptions =>
                     {
                         jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                         {
                             IssuerSigningKey = new SymmetricSecurityKey(seurityKey),
                             ValidIssuer = configuration["JwtConfig:Issuer"],
                             ValidAudience = configuration["JwtConfig:Audience"],
                         };
                     });
        }
    }
}
