using FluentValidation.Results;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SettlementBookingSystem.Application.Bookings.Services
{
	public class BookingService : IBookingService
	{
		private readonly List<Booking> _bookings = new();
		private readonly int _maxBookingsPerSlot = 4;
		private readonly object _locker = new object();

		public Booking CreateBooking(string name, string bookingTime)
		{
			// due to the singleton instance, need to lock to avoid race conditions 
			lock (_locker)
			{
				var errors = new List<ValidationFailure>();
				if (!TimeSpan.TryParse(bookingTime, out TimeSpan time))
				{
					errors.Add(new ValidationFailure("BookingTime", "Booking time is invalid"));

					throw new FluentValidation.ValidationException(errors);
				}

				if (time < new TimeSpan(9, 0, 0) || time > new TimeSpan(16, 0, 0))
				{
					errors.Add(new ValidationFailure("BookingTime", "Booking time is out of business hours"));

					throw new FluentValidation.ValidationException(errors);
				}

				var endTime = time.Add(TimeSpan.FromMinutes(59));

				var overlappingBookings = _bookings.Count(b =>
					(b.BookingTime <= endTime && b.BookingTime.Add(TimeSpan.FromMinutes(59)) >= time));

				if (overlappingBookings >= _maxBookingsPerSlot)
				{
					throw new ConflictException("All slots are booked for this time.");
				}

				var booking = new Booking
				{
					BookingId = Guid.NewGuid(),
					Name = name,
					BookingTime = time
				};

				_bookings.Add(booking);

				return booking;
			}
		}
	}
}
