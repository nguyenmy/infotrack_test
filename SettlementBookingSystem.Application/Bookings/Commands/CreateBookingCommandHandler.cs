using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Bookings.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
	public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
	{
		private IBookingService _bookingService;
		public CreateBookingCommandHandler(IBookingService bookingService)
		{
			_bookingService = bookingService;
		}

		public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
		{
			var booking = _bookingService.CreateBooking(request.Name, request.BookingTime);

			return new BookingDto(booking.BookingId);
		}
	}
}
