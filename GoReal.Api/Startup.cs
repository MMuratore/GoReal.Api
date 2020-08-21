using GoReal.Api.Infrastrucutre.Configuration;
using GoReal.Common.Interfaces;
using GoReal.Models.Entities;
using GoReal.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddControllers();

            IConfigurationSection jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            JwtBearerTokenSettings jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();

            services.AddSingleton<ITokenService, TokenService>(sp => new TokenService(jwtSection.Get<JwtBearerTokenSettings>().SecretKey));

            IConfigurationSection dbSection = Configuration.GetSection("DbConnectionSettings");
            DbConnectionSettings dbConnectionSettings = dbSection.Get<DbConnectionSettings>();
            string connectionString = dbSection.Get<DbConnectionSettings>().SqlServerConnectionString;

            services.AddSingleton<IAuthRepository<User>, AuthRepository>(sp => new AuthRepository(connectionString));
            services.AddSingleton<IRoleRepository<Role>, RoleRepository>(sp => new RoleRepository(connectionString));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
