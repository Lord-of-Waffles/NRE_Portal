using WebAPI_NRE_Portal.Services;

namespace WebAPI_NRE_Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IProductionService, ProductionService>();

            // CORS configuration - MOVED HERE (before builder.Build())
            const string CorsPolicy = "AllowLocal";
            builder.Services.AddCors(o =>
            {
                o.AddPolicy(CorsPolicy, p =>
                    p.WithOrigins(
                        "https://localhost:7288",
                        "http://localhost:5172",
                        "http://localhost:5002",
                        "https://localhost:5001",
                        "http://mvc:8080",           // Docker container name
                        "http://localhost:5002"       // Docker host access
                    )
                     .AllowAnyHeader()
                     .AllowAnyMethod());
            });

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseCors(CorsPolicy); // Enable CORS middleware

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}