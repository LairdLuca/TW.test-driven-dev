using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        public IRoomBookingService _roomBookingService { get; }

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            if(bookingRequest is null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }


            _roomBookingService.Save(CreateRoomBookingObject<RoomBooking>(bookingRequest));

            return CreateRoomBookingObject<RoomBookingResult>(bookingRequest);
        }

        private TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking : RoomBookingBase, new()
        {
            return new TRoomBooking
            {
                BookingDate = bookingRequest.BookingDate,
                Email = bookingRequest.Email,
                FullName = bookingRequest.FullName
            };
        }
    }
}