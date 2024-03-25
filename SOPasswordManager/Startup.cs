using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NToastNotify;
using NToastNotify.Libraries;

namespace SOPasswordManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<String>("EmailSetting:Host"),
                    Port = config.GetValue<int>("EmailSetting:Port"),
                    Credentials = new NetworkCredential(
                            config.GetValue<String>("EmailSetting:Username"),
                            config.GetValue<String>("EmailSetting:Password")
                        )
                };
            });

            services.AddSingleton(Configuration);
            #region session manage
            services.AddDistributedMemoryCache();


            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });

            #endregion
            services.AddDistributedMemoryCache();
            services.AddMvc();
            services.AddSignalR();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var cachePeriod = Environment.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });
            app.UseRouting();
            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
    /*
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<String>("EmailSetting:Host"),
                    Port = config.GetValue<int>("EmailSetting:Port"),
                    Credentials = new NetworkCredential(
                            config.GetValue<String>("EmailSetting:Username"),
                            config.GetValue<String>("EmailSetting:Password")
                        )
                };
            });
            services.AddMvc();

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue; //default 1024
                options.ValueLengthLimit = int.MaxValue; //not recommended value
                options.MultipartBodyLengthLimit = long.MaxValue; //not recommended value
            });
            

            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = false,
                PositionClass = ToastPositions.BottomCenter
            });
            
            services.AddSingleton<IConfiguration>(Configuration);
            #region session manage
            services.AddDistributedMemoryCache();


            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });

            #endregion

            #region Cache
            services.AddMemoryCache();
            //services.AddSingleton<Repo.ServiceContract.ILanguage_Repository, Repo.Service.Language_Repository>();

            #endregion

            services.AddMvc().AddNToastNotifyToastr();
            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //});

            services.AddSignalR();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error/Error500");
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error/Error500");
            }

            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles();

            //app.UseStatusCodePagesWithReExecute("/Home/Errors/{0}");

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                context.Context.Response.Headers.Add("Cache-Control", "public, max-age=2592000")
            });
            

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseNToastNotify();
            //Used before with .NET Core 2.1
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Login}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default", 
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
    */
}
