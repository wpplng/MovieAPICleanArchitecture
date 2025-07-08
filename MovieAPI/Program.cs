using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Data;
using MovieAPI.Services;

namespace MovieAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MovieContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MovieContext") ?? throw new InvalidOperationException("Connection string 'MovieContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddHostedService<DataSeedService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
