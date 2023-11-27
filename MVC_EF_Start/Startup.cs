#nullable enable
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC_EF_Start.DataAccess;

// https://stackoverflow.com/a/58072137/1385857
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.IO;
using System;
using System.Data;
using System.Data.OleDb;


namespace MVC_EF_Start
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup EF connection
            // https://stackoverflow.com/a/43098152/1385857
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:WAEVs:ConnectionString"]));

            // added from MVC template
            //services.AddMvc();
            // https://stackoverflow.com/a/58772555/1385857
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddSession();

            // Register the hosted service
            services.AddHostedService<DataImportService>();
        }
    }

        public class DataImportService : IHostedService
        {
            private readonly IServiceProvider _serviceProvider;

            public DataImportService(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    await context.SaveChangesAsync(); // Save changes to the database
                }
            }

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        }


        /*

            // this is the version from the MVC template
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //This ensures that the database and tables are created as per the Models.
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
            app.UseSession();

            // https://stackoverflow.com/a/58072137/1385857
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
              name: "default",
              template: "{controller=Home}/{action=MainPage}/{id?}");
            });
        }
        */
    }
}