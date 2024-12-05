using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core.Tests
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        public RoomBookingRequestProcessorTest()
        {
            _processor = new RoomBookingRequestProcessor();
        }

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

            // Act
            RoomBookingResult result = _processor.BookRoom(bookingRequest);

            // Assert
            //Assert.NotNull(result);         // without Shouldly
            //Assert.Equal(bookingRequest.FullName, result.FullName);
            //Assert.Equal(bookingRequest.Email, result.Email);
            //Assert.Equal(bookingRequest.BookingDate, result.BookingDate);

            result.ShouldNotBeNull();       // with Shouldly
            result.FullName.ShouldBe(bookingRequest.FullName);
            result.Email.ShouldBe(bookingRequest.Email);
            result.BookingDate.ShouldBe(bookingRequest.BookingDate);

        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            // Arrange
            //var processor = new RoomBookingRequestProcessor();

            // Act
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));

            // Assert
            exception.ParamName.ShouldBe("bookingRequest");
        }
    }
}
