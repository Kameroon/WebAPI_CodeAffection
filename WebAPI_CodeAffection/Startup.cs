using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using WebAPI_CodeAffection.Models;

namespace WebAPI_CodeAffection
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
            //// -- De base on ne renvoie pas les champs exactement comme décrit dans la classe --
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // -- Permet de renvoyer les données au format de création  --
            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
               .AddJsonOptions(options =>
               {
                   var resolver = options.SerializerSettings.ContractResolver;
                   if (resolver != null)
                       (resolver as DefaultContractResolver).NamingStrategy = null;
               });
           

            // -- Injection de dépance du contexte --
            services.AddDbContext<PaymentDetailContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));


            // Ajouter pour permettre la consomation de celui-ci par Angular
            // pui on le configuire plus bas
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region -- Configuration pour consomation par Angular project --
            //app.UseCors(options =>
            //            options.WithOrigins("http://localhost:4200/")
            //                   .AllowAnyMethod()
            //                   .AllowAnyHeader());

            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader());
            #endregion

            app.UseMvc();
        }
    }
}
