using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sklepik.Data;

[assembly: HostingStartup(typeof(Sklepik.Areas.Identity.IdentityHostingStartup))]
namespace Sklepik.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                
                services.AddIdentity<IdentityUser, IdentityRole>(
                    options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.Password.RequiredLength = 3;
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;

                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            });
        }
    }
}