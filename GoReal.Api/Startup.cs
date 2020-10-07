using GoReal.Api.Infrastrucutre.Configuration;
using GoReal.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using System.Data.SqlClient;
using Tools.Databases;
using Tools.Security.Token;
using GoReal.Dal.Entities;

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
                        builder.WithOrigins("https://localhost:4200", "http://localhost:4200");
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

            services.AddSingleton<AuthService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<RoleService>();
            services.AddSingleton<GameService>();
            services.AddSingleton<RuleService>();
            services.AddSingleton<TimeControlService>();
            services.AddSingleton<StatisticService>();
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
