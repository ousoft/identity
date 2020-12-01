using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oyang.Identity.Infrastructure.Common;
using Oyang.Identity.IApplication.Database;
using Oyang.Identity.Application.Database;
using Oyang.Identity.IApplication.Account;
using Oyang.Identity.Application.Account;
using Oyang.Identity.IApplication.User;
using Oyang.Identity.Application.User;
using Oyang.Identity.IApplication.Role;
using Oyang.Identity.Application.Role;

namespace Oyang.Identity.WebApi
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOyangIdentity(this IServiceCollection services)
        {
            var currentUser = new CurrentUser(Guid.Empty, "admin", null, null);
            services.AddScoped<CurrentUser>(t => currentUser);
            services.AddScoped<ICurrentUser>(t => currentUser);
            services.AddScoped<IDatabaseAppService, DatabaseAppService>();
            services.AddScoped<IAccountAppService, AccountAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            return services;
        }
    }
}
