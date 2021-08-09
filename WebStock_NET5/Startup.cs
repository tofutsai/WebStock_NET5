
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;
using WebStock_NET5.DAL;
using WebStock_NET5.DB;
using WebStock_NET5.Filter;
using WebStock_NET5.Hubs;

namespace WebStock_NET5
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

            //�]�mJson��X�榡
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            ////�]�mSignalR������v��
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.SetIsOriginAllowed(origin => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                     .AllowCredentials();
            }));

            //�[�JSignalR�A��
            services.AddSignalR();

            //Scaffold-DbContext "Server=LAPTOP-82BMHD1B\SQLEXPRESS;Database=WebStock;User ID=sa;Password=Passw0rd;Integrated Security=False;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DB -Force -UseDatabaseNames

            //��Ʈw����DI�`�J
            services.AddDbContext<WebStockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WebStockDatabase")));
            services.AddDbContext<WebStockContextDTO>(options => options.UseSqlServer(Configuration.GetConnectionString("WebStockDatabase")));

            //DAL Method DI�`�J
            services.AddScoped<IStockIndexDAL, StockIndexDAL>();
            services.AddScoped<IStockDataDAL, StockDataDAL>();
            services.AddScoped<IStockSysConfigDAL, StockSysConfigDAL>();
            services.AddScoped<IStockSysLogDAL, StockSysLogDAL>();
            services.AddScoped<IStockStatisticsDAL, StockStatisticsDAL>();
            services.AddScoped<IStockFavoriteDAL, StockFavoriteDAL>();
            services.AddScoped<IStockProfitDAL, StockProfitDAL>();
            services.AddScoped<IStockDownloadDAL, StockDownloadDAL>();
            services.AddScoped<IStockComputeDAL, StockComputeDAL>();
            services.AddScoped<IStockMemoDAL, StockMemoDAL>();
            services.AddScoped<IStockMemberDAL, StockMemberDAL>();
            services.AddControllers();
            
            //MVC �A�Ȥ����U Filter
            services.AddMvc(config =>
            {
                config.Filters.Add(new ActionFilter());
                config.Filters.Add(new ExceptionFilter());
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //�]�m�R�A����
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "vue")),
                RequestPath = "/vue",
                EnableDefaultFiles = true
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //�]�mCors site
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
