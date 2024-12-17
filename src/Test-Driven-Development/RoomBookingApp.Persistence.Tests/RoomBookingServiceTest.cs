using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {

        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // Arrange
            var date = new DateTime(2021, 1, 1);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase(databaseName: "RoomBookingDb")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            context.Add(new Room { Id = 1, Name = "Conference Room A" });
            context.Add(new Room { Id = 2, Name = "Conference Room B" });
            context.Add(new Room { Id = 3, Name = "Conference Room C" });

            context.Add(new RoomBooking { Id = 1, RoomId = 1, BookingDate = date });
            context.Add(new RoomBooking { Id = 2, RoomId = 2, BookingDate = date.AddDays(-1) });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // Act
            var availableRooms = roomBookingService.GetAvailableRooms(DateTime.Now);

            // Assert
            Assert.Equal(availableRooms.Count(), 2);
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
        }
    }
}
