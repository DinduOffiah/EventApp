using EventApp.Core.DTO;
using EventApp.Core.Services;
using EventApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.API.Controllers
{
    /// <summary>
    /// Controller for managing events.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventService _service;

        /// <summary>
        /// Initializes a new instance of the EventController class.
        /// </summary>
        /// <param name="service">The service used to manage event.</param>
        /// <param name="logger">This is for logging</param>
        public EventsController(IEventService service, ILogger<EventsController> logger)
        {
            _service = service;
            _logger = logger;
        }


        /// <summary>
        /// This endpoint gets LIST of events.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = await _service.GetEventAsync();

                if (events == null || !events.Any())
                {
                    return NotFound("No event found.");
                }

                var eventDtos = events.Select(e => new EventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    Image = e.Image,
                    EventDate = e.EventDate,
                    Location = e.Location,
                    TicketTypeId = e.TicketTypeId,
                    EventTypeId = e.EventTypeId,
                    Limit = e.Limit,
                    EventTypeName = e.EventType?.EventTypeName,
                    TicketTypeName = e.TicketType?.TicketTypeName,
                }).ToList();

                return Ok(eventDtos);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                _logger.LogError(ex, "An error occurred while getting events.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint gets DETAILS of an event by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            try
            {
                var eventItem = await _service.GetEventByIdAsync(id);

                if (eventItem == null)
                {
                    return NotFound($"Event with id {id} not found.");
                }

                var eventDto = new EventDto
                {
                    EventName = eventItem.EventName,
                    Image = eventItem.Image,
                    EventDate = eventItem.EventDate,
                    Location = eventItem.Location,
                    TicketTypeId = eventItem.TicketTypeId,
                    EventTypeId = eventItem.EventTypeId,
                    Limit = eventItem.Limit
                };

                return Ok(eventDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the event with id {id}.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint CREATES events.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto eventDto)
        {
            try
            {
                var eventItem = new Event
                {
                    EventName = eventDto.EventName,
                    Image = eventDto.Image,
                    EventDate = eventDto.EventDate,
                    Location = eventDto.Location,
                    TicketTypeId = eventDto.TicketTypeId,
                    EventTypeId = eventDto.EventTypeId,
                    Limit = eventDto.Limit,
                };

                var newEvent = await _service.CreateEventAsync(eventItem);

                if (newEvent == null)
                {
                    return BadRequest("Error creating event.");
                }

                var newEventDto = new EventDto
                {
                    EventName = newEvent.EventName,
                    Image = newEvent.Image,
                    EventDate = newEvent.EventDate,
                    Location = newEvent.Location,
                    TicketTypeId = newEvent.TicketTypeId,
                    EventTypeId = newEvent.EventTypeId,
                    Limit = newEvent.Limit
                };

                return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, newEventDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the event.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint UPDATES/EDITS events.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDto eventDto)
        {
            try
            {
                var eventItem = await _service.GetEventByIdAsync(id);
                if (eventItem == null)
                {
                    return NotFound($"Event with id {id} not found.");
                }

                eventItem.EventName = eventDto.EventName ?? eventItem.EventName;
                eventItem.Image = eventDto.Image ?? eventItem.Image;
                eventItem.EventDate = eventDto.EventDate ?? eventItem.EventDate;
                eventItem.Location = eventDto.Location ?? eventItem.Location;
                eventItem.TicketTypeId = eventDto.TicketTypeId != 0 ? eventDto.TicketTypeId : eventItem.TicketTypeId;
                eventItem.EventTypeId = eventDto.EventTypeId != 0 ? eventDto.EventTypeId : eventItem.EventTypeId;
                eventItem.Limit = eventDto.Limit != 0 ? eventDto.Limit : eventItem.Limit;

                await _service.UpdateEventAsync(eventItem);

                return Ok(eventItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the event with id {id}.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint DELETES events.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var eventToDelete = await _service.GetEventByIdAsync(id);
                if (eventToDelete == null)
                {
                    return NotFound($"Event with id {id} not found.");
                }

                var eventDto = new EventDto
                {
                    EventName = eventToDelete.EventName,
                    Image = eventToDelete.Image,
                    EventDate = eventToDelete.EventDate,
                    Location = eventToDelete.Location,
                    TicketTypeId = eventToDelete.TicketTypeId,
                    EventTypeId = eventToDelete.EventTypeId,
                    Limit = eventToDelete.Limit
                };

                await _service.DeleteEventAsync(id);

                return Ok(eventDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the event with id {id}.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
