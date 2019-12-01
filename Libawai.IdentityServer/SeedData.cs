using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Libawai.IdentityServer.Data;
using Libawai.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Libawai.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            //1.dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
            //2.dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
            //3.dotnet ef migrations add AppDbMigration -c ApplicationDbContext -o Data
            //4.dotnet run /seed

            Console.WriteLine("Seeding database...");
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                .Database.Migrate();

            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            configurationDbContext.Database.Migrate();

            if (!configurationDbContext.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (Client client in Config.GetClients().ToList())
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources beging populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                Console.WriteLine("ApiResources beging populated");
                foreach (var resource in Config.GetApiResources().ToList())
                {
                    configurationDbContext.ApiResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }

            var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            applicationDbContext.Database.Migrate();
            
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "alice"
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Console.WriteLine("alice created");
            }
            else
            {
                Console.WriteLine("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob"
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Console.WriteLine("bob created");
            }
            else
            {
                Console.WriteLine("bob already exists");
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}
