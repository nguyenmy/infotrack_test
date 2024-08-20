using SettlementBookingSystem.Application.Models;
using System;

namespace SettlementBookingSystem.Application.Bookings.Services
{
	public interface IBookingService
	{
		Booking CreateBooking(string name, string bookingTime);
	}
}
