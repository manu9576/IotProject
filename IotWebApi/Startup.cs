using IotWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

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


			services.AddControllers();
			services.AddMvc();

			services.AddDbContext<DbSensorsContext>
			(
				opt => opt.UseMySql
				(
					connectionStrings.MySql,
					options => options.EnableRetryOnFailure()
				)
			);

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo 
				{ 
					Title = ".Net Core 3.1 Iot Web API", 
					Version = "v1" ,
					Description = "An ASP.NET Core API that allows to manage IOT measurements.",
					Contact = new OpenApiContact
					{
						Name = "Emmanuel Lemaistre",
						Email = "Manu9576@hotmail.fr",
						Url = new Uri("https://manu9576.net"),
					},
					License = new OpenApiLicense
					{
						Name = "MIT License",
						Url = new Uri("https://opensource.org/licenses/MIT"),
					}
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

				options.IncludeXmlComments(xmlPath);
			}).AddSwaggerGenNewtonsoftSupport();

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

			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", ".Net Core 3.1 Iot Web API V1"));
			}
		}
	}
}
