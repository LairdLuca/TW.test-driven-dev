
namespace RoomBookingApp.Core.Tests
{
    internal class RoomBookingRequestProcessor
    {
        public RoomBookingRequestProcessor()
        {
        }

        internal RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            return new RoomBookingResult
            {
                BookingDate = bookingRequest.BookingDate,
                Email = bookingRequest.Email,
                FullName = bookingRequest.FullName
            };
        }
    }
}