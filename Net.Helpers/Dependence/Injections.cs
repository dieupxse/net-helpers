using System.Net.Mime;
using System;
using Net.Helpers.Implements;
using Net.Helpers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Net.Helpers.Dependence
{
    public static class Injections
    {
        public static IServiceCollection AddInjection(this IServiceCollection services)
        {
            services.AddScoped<ILogHelper, LogHelper>();
            services.AddScoped<ICacheHelper, CacheHelper>();
            services.AddScoped<IStringHelper, StringHelper>();
            services.AddScoped<ISecurityHelper, SecurityHelper>();
            services.AddScoped<IApiHelper, ApiHelper>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<IImageHelper, ImageHelper>();
            return services;
        }
    }
}
