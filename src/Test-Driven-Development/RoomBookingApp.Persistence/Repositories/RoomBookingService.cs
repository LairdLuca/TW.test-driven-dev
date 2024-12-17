using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private RoomBookingAppDbContext _context;

        public RoomBookingService(RoomBookingAppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            //var unAvailableRooms = _context.RoomBookings.Where(q => q.BookingDate == date.Date).Select(q => q.Room);
            //var availableRooms = _context.Rooms.Where(q => !unAvailableRooms.Any(r => r.Id == q.Id));

            var availableRooms = _context.Rooms.Where(q => !q.RoomBookings.Any(x => x.BookingDate == date.Date));

            return availableRooms;
        }

        public void Save(RoomBooking roomBooking)
        {
            _context.RoomBookings.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
