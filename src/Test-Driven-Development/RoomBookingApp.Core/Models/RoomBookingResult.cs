﻿using RoomBookingApp.Core.Enums;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingResult : RoomBookingBase
    {
        public BookingSuccessFlag Flag { get; set; }
        public int? RoomBookingId { get; set; }

    }
}