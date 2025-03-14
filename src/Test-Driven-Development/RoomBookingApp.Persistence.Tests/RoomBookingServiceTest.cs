﻿using Microsoft.EntityFrameworkCore;
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

            context.Add(new RoomBooking { Id = 1, RoomId = 1, BookingDate = date, Email = "", FullName = "" });
            context.Add(new RoomBooking { Id = 2, RoomId = 2, BookingDate = date.AddDays(-1), Email = "", FullName = "" });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // Act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // Assert
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldSaveTest")
                .Options;

            var roomBooking = new RoomBooking
            {
                RoomId = 1,
                BookingDate = new DateTime(2021, 1, 1),
                Email = "",
                FullName = ""
            };

            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);
            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(roomBooking.BookingDate, booking.BookingDate);
            Assert.Equal(roomBooking.Email, booking.Email);
            Assert.Equal(roomBooking.FullName, booking.FullName);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);

        }
    }
}
