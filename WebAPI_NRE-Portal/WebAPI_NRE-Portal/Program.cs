using DataLayer_NRE_Portal.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI_NRE_Portal.Services;
using WebAPI_NRE_Portal.Data;

namespace WebAPI_NRE_Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<NrePortalContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IProductionService, ProductionService>();

            const string CorsPolicy = "AllowLocal";
            builder.Services.AddCors(o =>
            {
                o.AddPolicy(CorsPolicy, p =>
                    p.WithOrigins(
                        "https://localhost:7288",
                        "http://localhost:5172",
                        "http://localhost:5002",
                        "https://localhost:5001")
                     .AllowAnyHeader()
                     .AllowAnyMethod());
            });

            var app = builder.Build();

            // seed
            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<NrePortalContext>();
                ctx.Database.Migrate();
                DbSeeder.Seed(ctx);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(CorsPolicy);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
