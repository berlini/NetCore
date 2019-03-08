using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoTheDishesWebservice.Core.Services;
using DoTheDishesWebservice.DataAccess.Configurations;
using DoTheDishesWebservice.DataAccess.Repositories;
using DoTheDishesWebservice.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace DoTheDishesWebservice
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configure EFCore
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=dishes;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DishesContext>(options => options.UseSqlServer(connectionString));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Do The Dishes API", Version = "v1" });
            });

            // Configure Repositories
            services.AddScoped<IUserRespository, UserRepository>();
            services.AddScoped<IHomeRepository, HomeRepoitory>();

            // Configure Services
            services.AddScoped<IUserService, UsersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Do The Dishes API V1");
            });

            app.UseMvc();
        }
    }
}
