using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        public RoomBookingRequestProcessor()
        {
        }

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
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