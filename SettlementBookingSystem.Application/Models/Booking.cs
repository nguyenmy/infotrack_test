using System;
namespace SettlementBookingSystem.Application.Models
{
	public class Booking
	{
		public Guid BookingId { get; set; }
		public string Name { get; set; }
		public TimeSpan BookingTime { get; set; }
	}
}
