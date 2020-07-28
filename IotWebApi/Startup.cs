using IotWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;

namespace IotWebApi
{
    /// <summary>
    /// Class that create the service
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostEnvironment env)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            IConfigurationRoot appSettings = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath).AddJsonFile(Path.Combine(path, "appsettings.json")).Build();

            Configuration = appSettings;
        }


        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionStrings connectionStrings = new ConnectionStrings();
            Configuration.Bind("ConnectionStrings", connectionStrings);
            services.AddSingleton(connectionStrings);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost");
                                  });
            });

            services.AddControllers();
            services.AddMvc();

            services.AddDbContext<DbSensorsContext>(opt => opt.UseMySql(connectionStrings.MySql));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ".Net Core 3 Iot Web API", Version = "v1" });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ".Net Core 3 Web API V1");
            });

#endif
        }
    }
}
