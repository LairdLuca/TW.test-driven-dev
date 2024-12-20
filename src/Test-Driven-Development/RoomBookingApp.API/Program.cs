
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;

namespace RoomBookingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // db context in memory for testing
            var connString = "DataSource=:memory";
            var conn = new SqliteConnection(connString);
            conn.Open();

            builder.Services.AddDbContext<RoomBookingAppDbContext>(options =>
            {
                options.UseSqlite(conn);
            });

            builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();

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
