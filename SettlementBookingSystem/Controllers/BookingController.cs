using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Dtos;
using System;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

		/// <summary>
		/// Creates a new booking for a settlement.
		/// </summary>
		/// <param name="request">The booking request containing the time and name.</param>
		/// <returns>A response with a booking ID if successful, or an error message if not.</returns>
		/// <response code="200">Returns the booking ID</response>
		/// <response code="400">If the input data is invalid</response>
		/// <response code="409">If all settlement slots are booked at the requested time</response>
		/// <response code="500">If an internal server exception occurs</response>
		[HttpPost]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingCommand command)
        {
            var result =  await _mediator.Send(command);
            return Ok(result);
        }
    }
}
