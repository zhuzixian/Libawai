using System.Linq;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Libawai.API.Extensions;
using Libawai.Core.Interfaces;
using Libawai.Infrastructure.Database;
using Libawai.Infrastructure.Repositories;
using Libawai.Infrastructure.Resources;
using Libawai.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Libawai.API
{
    public class StartupDevelopment
    {
        public static IConfiguration Configuration { get; private set; }

        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }) .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new ExpandoObjectConverter());
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddFluentValidation();

            services.Configure<MvcOptions>(options =>
            {
                var inputFormatter = options.InputFormatters.OfType<NewtonsoftJsonInputFormatter>()
                    .FirstOrDefault();
                if (inputFormatter != null)
                {
                    inputFormatter.SupportedMediaTypes.Add("application/vnd.libawai.post.create+json");
                    inputFormatter.SupportedMediaTypes.Add("application/vnd.libawai.post.update+json");
                }

                var outputFormatter = options.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()
                    .FirstOrDefault();
                outputFormatter?.SupportedMediaTypes.Add("application/vnd.libawai.hateoas+json");

            });


            services.AddDbContext<LibawaiDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

           services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 4841;
            });

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostImageRepository, PostImageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddTransient<IValidator<PostAddResource>, 
                PostAddOrUpdateResourceValidator<PostAddResource>>();
            services.AddTransient<IValidator<PostUpdateResource>,
                PostAddOrUpdateResourceValidator<PostUpdateResource>>();
            services.AddTransient<IValidator<PostImageResource>,
                PostImageResourceValidator>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext; 
                return factory.GetService<IUrlHelperFactory>().GetUrlHelper(actionContext);
            });

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<PostPropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDevOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:4842";
                    options.RequireHttpsMetadata = false;

                    options.Audience = "api1";
                });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseLibawaiExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();
            app.UseCors("AllowAngularDevOrigin");
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}
