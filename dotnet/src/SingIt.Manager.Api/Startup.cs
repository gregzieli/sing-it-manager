using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Newtonsoft.Json;
using SingIt.Manager.Api.Infrastructure;
using SingIt.Manager.Api.Infrastructure.Configuration;
using SingIt.Manager.Api.Services;

namespace SingIt.Manager.Api
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
            services.AddTransient<SongService>();
            ConfigureDatabase(services);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //options.UseMemberCasing();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sing It Manager API", Version = "v1" });
            });
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sing It Manager API V1");
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            // TODO: Put all of this in extension methods services.ConfigureMongoDb(Action<SomeCustomHandler> builder)
            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            var mongoUrl = MongoUrl.Create(Configuration.GetConnectionString("SingIt"));

            services.AddSingleton<IMongoClient>(x => new MongoClient(mongoUrl));
            services.AddScoped(x => x.GetRequiredService<IMongoClient>().GetDatabase(mongoUrl.DatabaseName));
            services.AddScoped(x => new ManagerContext(x.GetRequiredService<IMongoDatabase>()));

            SongTypeConfiguration.Configure();
        }
    }
}
