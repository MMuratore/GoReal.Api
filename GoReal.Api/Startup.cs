using GoReal.Api.Infrastrucutre.Configuration;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services;
using GoReal.Services.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using System.Data.SqlClient;
using Tools.Databases;
using Tools.Security.Token;

namespace GoReal.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("localhost",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:4200");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });

            services.AddControllers();

            IConfigurationSection jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            JwtBearerTokenSettings jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();

            services.AddSingleton<ITokenService, TokenService>(sp => new TokenService(jwtSection.Get<JwtBearerTokenSettings>().SecretKey));

            IConfigurationSection dbSection = Configuration.GetSection("DbConnectionSettings");
            DbConnectionSettings dbConnectionSettings = dbSection.Get<DbConnectionSettings>();
            string connectionString = dbSection.Get<DbConnectionSettings>().SqlServerConnectionString;
            
            services.AddSingleton<DbProviderFactory>(sp => SqlClientFactory.Instance);
            services.AddSingleton(sp => new ConnectionInfo(connectionString));
            services.AddSingleton<Connection>();

            services.AddSingleton<IAuthRepository<User>, AuthRepository>();
            services.AddSingleton<IUserRepository<User>, UserRepository>();
            services.AddSingleton<IRoleRepository<Role>, RoleRepository>();

            services.AddSingleton<GameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
