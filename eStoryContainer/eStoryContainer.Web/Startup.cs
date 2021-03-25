using eStoryContainer.Core.Interfaces;
using eStoryContainer.Data;
using eStoryContainer.Models;
using eStoryContainer.Services.Categories;
using eStoryContainer.Services.Chapters;
using eStoryContainer.Services.Stories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace eStoryContainer.Web
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
            //services.AddControllers();

            services.Configure<ScanStoryDB>(Configuration.GetSection("ScanStorySettings"));

            services.AddSingleton<IScanStoryDB>(sp => sp.GetRequiredService<IOptions<ScanStoryDB>>().Value);

            services.AddSingleton<ApplicationDbContext>();

            services.AddScoped<IStoryService, StoryService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryStoryService, CategoryStoryService>();

            services.AddMemoryCache();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("stories", "{slug}", new { controller = "Stories", action = "Index" });
                endpoints.MapControllerRoute("stories", "/{slug}/trang-{index}/#list-chapter", new { controller = "Stories", action = "Index" });
                endpoints.MapControllerRoute("detail", "/chi-tiet/{slug}", new { controller = "Chapters", action = "Index" });
            });
        }
    }
}
