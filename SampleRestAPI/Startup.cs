using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;
using SampleRestAPI.API.Domain.Repositories;
using SampleRestAPI.API.Domain.Services;
using SampleRestAPI.Persistence;
using SampleRestAPI.API.Persistence.Contexts;
using SampleRestAPI.API.Persistence.Repositories;
using SampleRestAPI.API.Services;
using Microsoft.EntityFrameworkCore;

namespace SampleRestAPI
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
            string dbName = "TestDb";
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: dbName));

            //var context = services.GetRequiredService<AppDbContext>();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase<AppDbContext>(dbName);

            var context = new AppDbContext(builder.Options);
            if (!context.Movies.Any())
            {
                var tds = new TestDataSeeder(context);
                tds.SeedData();
            }

            var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfileConfiguration()));
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMemoryCache, MemoryCache>();
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
