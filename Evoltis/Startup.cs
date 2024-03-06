using Evoltis.Entity;
using Evoltis.Helpers;
using Evoltis.Models;
using Evoltis.Models.Dtos.ClubDtos;
using Evoltis.Models.Validator;
using Evoltis.Repositories;
using Evoltis.Repositories.Interfaces;
using Evoltis.Services;
using Evoltis.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Evoltis
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiEnvoltis", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));

            // Utilidades
            services.AddSingleton<ICrypto, Crypto>();
            services.AddScoped<IDbOperation, DbOperation>();
            services.AddScoped<IHelperFile, HelperFile>();

            //Servicios
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<ITournamentService, TournamentService>();
            //Repositorios
            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();

            //Validator
            services.AddTransient<IValidator<ClubCreateDto>, ClubCreateDtoValidator>();
            services.AddTransient<IValidator<ClubPatchDto>, ClubPatchDtoValidator>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiEnvoltis V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "wwwroot", "Images")),
                RequestPath = "/Images"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
