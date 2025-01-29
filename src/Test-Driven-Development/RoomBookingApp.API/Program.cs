
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "RoomBookingApp.API", Version = "v1" });
            });

            // db context in memory for testing
            var connString = "Filename=:memory:";
            var conn = new SqliteConnection(connString);
            conn.Open();

            builder.Services.AddDbContext<RoomBookingAppDbContext>(opt => opt.UseSqlite(conn));

            EnsureDatabaseCreated(conn);

            builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
            builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RoomBookingApp.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void EnsureDatabaseCreated(SqliteConnection conn)
        {
            var builder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
            builder.UseSqlite(conn);

            using var context = new RoomBookingAppDbContext(builder.Options);
            context.Database.EnsureCreated();
        }
    }
}
