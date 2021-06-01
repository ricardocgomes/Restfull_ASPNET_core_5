using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mvc.Configurations;
using Mvc.Hypermedia.Enricher;
using Mvc.Repository.Implementations;
using Mvc.Repository.Interfaces;
using Mvc.Services.Authentication;
using Mvc.Services.Authentication.Implementations;
using Mvc.Services.Implementations;
using Mvc.Services.Interfaces;
using MVC.Business;
using MVC.Business.Implementations;
using MVC.Hypermedia.Enricher;
using MVC.Hypermedia.Filter;
using MVC.Model.Context;
using MVC.Repository;
using MVC.Repository.Implementations;
using MVC.Services.Implementations;
using MVC.Services.Interfaces;
using Serilog;
using System;
using System.Text;

namespace MVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenConfiguration = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                Configuration.GetSection("TokenConfigurations")

                ).Configure(tokenConfiguration);

            services.AddSingleton(tokenConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidAudience = tokenConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddCors(op => op.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SqlServerContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            if (Environment.IsDevelopment())
            {
                MigrateDataBase(connection);
            }

            //Versioning API
            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Titulo",
                        Version = "v1",
                        Description = "Description",
                        Contact = new OpenApiContact
                        {
                            Email = "email",
                            Name = "name",

                        }
                    });
            });

            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BooksEnricher());

            //Dependency Injection
            services.AddSingleton(filterOptions);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILoginRepository, LoginRepository>();

            services.AddTransient<IFileService, FileService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IBooksService, BooksService>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));            
        }

        private static void MigrateDataBase(string connection)
        {
            try
            {
                SqlConnection cnx = new(connection);
                var evolve = new Evolve.Evolve(cnx, msg => Log.Information(msg))
                {
                    Locations = new[] { "db/migrations", "db/dataSet" },
                    IsEraseDisabled = true,
                };

                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed.", ex);
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "swagger");
            });

            RewriteOptions options = new();
            options.AddRedirect("^$", "swagger");
            app.UseRewriter(options);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });
        }
    }
}
