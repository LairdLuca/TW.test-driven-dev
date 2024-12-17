using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using Shouldly;

namespace RoomBookingApp.Core.Tests
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;

        public RoomBookingRequestProcessorTest()
        {
            _request = new RoomBookingRequest
            {
                BookingDate = DateTime.Now,
                FullName = "Test Name",
                Email = "test@request.com",
            };

            _availableRooms = new List<Room> { new Room() { Id = 1 } };

            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock
                .Setup(x => x.GetAvailableRooms(It.IsAny<DateTime>()))
                .Returns(_availableRooms);

            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
        }

        [Fact]
        public void Should_Return_Room_Booking_Response_With_Request_Values()
        {
            // Arrange
            //var bookingRequest = new RoomBookingRequest
            //{
            //    BookingDate = DateTime.Now,
            //    FullName = "Test Name",
            //    Email = "test@request.com",
            //};

            // Act
            RoomBookingResult result = _processor.BookRoom(_request);

            // Assert
            //Assert.NotNull(result);         // without Shouldly
            //Assert.Equal(bookingRequest.FullName, result.FullName);
            //Assert.Equal(bookingRequest.Email, result.Email);
            //Assert.Equal(bookingRequest.BookingDate, result.BookingDate);

            result.ShouldNotBeNull();       // with Shouldly
            result.FullName.ShouldBe(_request.FullName);
            result.Email.ShouldBe(_request.Email);
            result.BookingDate.ShouldBe(_request.BookingDate);

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

        [Fact]
        public void Should_Save_Room_Booking_Request()
        {
            RoomBooking savedRoomBooking = null;
            _roomBookingServiceMock
                .Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    savedRoomBooking = booking;
                });

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);

            savedRoomBooking.ShouldNotBeNull();
            savedRoomBooking.FullName.ShouldBe(_request.FullName);
            savedRoomBooking.Email.ShouldBe(_request.Email);
            savedRoomBooking.BookingDate.ShouldBe(_request.BookingDate);
            savedRoomBooking.RoomId.ShouldBe(_availableRooms.First().Id);
        }

        [Fact]
        public void Should_Not_Save_Room_Booking_Request_If_none_Available()
        {
            _availableRooms.Clear();
            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingSuccessFlag.Failure, false)]
        [InlineData(BookingSuccessFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingSuccessFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }

            var result = _processor.BookRoom(_request);
            bookingSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            else
            {
                _roomBookingServiceMock
                    .Setup(x => x.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking =>
                    {
                        booking.Id = roomBookingId.Value;
                    });
            }
            var result = _processor.BookRoom(_request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }
    }
}
