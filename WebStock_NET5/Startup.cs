
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

            //設置Json輸出格式
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            ////設置SignalR跨網域權限
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.SetIsOriginAllowed(origin => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                     .AllowCredentials();
            }));

            //加入SignalR服務
            services.AddSignalR();

            //Scaffold-DbContext "Server=LAPTOP-82BMHD1B\SQLEXPRESS;Database=WebStock;User ID=sa;Password=Passw0rd;Integrated Security=False;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DB -Force -UseDatabaseNames

            //資料庫物件的DI注入
            services.AddDbContext<WebStockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WebStockDatabase")));
            services.AddDbContext<WebStockContextDTO>(options => options.UseSqlServer(Configuration.GetConnectionString("WebStockDatabase")));

            //DAL Method DI注入
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
            
            //MVC 服務中註冊 Filter
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

            //設置靜態網頁
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "vue")),
                RequestPath = "/vue",
                EnableDefaultFiles = true
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //設置Cors site
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
