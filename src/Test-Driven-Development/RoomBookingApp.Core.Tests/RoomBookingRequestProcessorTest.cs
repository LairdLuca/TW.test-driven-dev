using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace RoomBookingApp.Core.Tests
{
    public class RoomBookingRequestProcessorTest
    {
        [Fact]
        public void Should_Return_Room_Booking_Response_With_Request_Values()
        {
            // Arrange
            var bookingRequest = new RoomBookingRequest
            {
                BookingDate = DateTime.Now,
                FullName = "Test Name",
                Email = "test@request.com",
            };

            var processor = new RoomBookingRequestProcessor();

            // Act
            RoomBookingResult result = processor.BookRoom(bookingRequest);

            // Assert
            Assert.NotNull(result);         // without Shouldly
            Assert.Equal(bookingRequest.FullName, result.FullName);
            Assert.Equal(bookingRequest.Email, result.Email);
            Assert.Equal(bookingRequest.BookingDate, result.BookingDate);

            result.ShouldNotBeNull();       // with Shouldly
            result.FullName.ShouldBe(bookingRequest.FullName);
            result.Email.ShouldBe(bookingRequest.Email);
            result.BookingDate.ShouldBe(bookingRequest.BookingDate);

        }
    }
}
