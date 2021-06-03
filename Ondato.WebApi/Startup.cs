using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ondato.Application;
using Ondato.Application.Abstractions;
using Ondato.Application.CleanupPolicies;
using Ondato.Application.KeyValueStores;
using Ondato.Application.LifetimePolicies;
using Ondato.Core.Configuration;
using Ondato.Data;
using ondato.Validators;

namespace ondato
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddDbContext<KeyValueStoreContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("KeyValueStoreContext")));

            services.AddFluentValidation(fv => fv
                .RegisterValidatorsFromAssemblyContaining<KeyValueValidator>());

            services.AddSwaggerGen();
            services.Configure<OndatoConfig>(Configuration.GetSection(OndatoConfig.Section));
            services.AddScoped<IOndatoDictionaryService, OndatoDictionaryService>();
            services.AddScoped(typeof(ControlledLifetimeStore<,>));
            services.AddSingleton<ILifetimePolicy, DefaultLifetimePolicy>();
            services.AddSingleton(typeof(ICleanupPolicy<>), typeof(ExpirationBasedPolicy<>));

            services.AddScoped(typeof(KeyValueStoreFactory));
            services.AddScoped(typeof(DatabaseKeyValueStore<,>));
            services.AddSingleton(typeof(InMemoryKeyValueStore<,>));
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
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Ondato Dictionary API";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}