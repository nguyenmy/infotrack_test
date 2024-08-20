using FluentAssertions;
using FluentValidation;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Services;
using SettlementBookingSystem.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
	public class CreateBookingCommandHandlerTests
	{
		[Fact]
		public async Task GivenValidBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
		{
			var command = new CreateBookingCommand
			{
				Name = "test",
				BookingTime = "09:15",
			};

			var handler = new CreateBookingCommandHandler(new BookingService());

			var result = await handler.Handle(command, CancellationToken.None);

			result.Should().NotBeNull();
			result.BookingId.Should().NotBeEmpty();
		}

		[Fact]
		public async Task GivenLatestBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
		{
			var command = new CreateBookingCommand
			{
				Name = "test",
				BookingTime = "16:00",
			};

			var handler = new CreateBookingCommandHandler(new BookingService());

			var result = await handler.Handle(command, CancellationToken.None);

			result.Should().NotBeNull();
			result.BookingId.Should().NotBeEmpty();
		}

		[Fact]
		public void GivenOutOfHoursBookingTime_WhenBooking_ThenValidationFails()
		{
			var command = new CreateBookingCommand
			{
				Name = "test",
				BookingTime = "00:00",
			};

			var handler = new CreateBookingCommandHandler(new BookingService());

			Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

			act.Should().Throw<ValidationException>();
		}

		[Fact]
		public void GivenValidBookingTime_WhenBookingIsFull_ThenConflictThrown()
		{
			string bookingName = "test";

			var bookingService = new BookingService();
			bookingService.CreateBooking(bookingName, "09:15");
			bookingService.CreateBooking(bookingName, "09:20");
			bookingService.CreateBooking(bookingName, "09:25");
			bookingService.CreateBooking(bookingName, "09:30");

			var command = new CreateBookingCommand
			{
				Name = "new booking",
				BookingTime = "09:35",
			};

			var handler = new CreateBookingCommandHandler(bookingService);

			Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

			act.Should().Throw<ConflictException>();
		}
	}
}
