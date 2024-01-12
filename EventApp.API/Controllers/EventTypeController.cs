using EventApp.Core.DTO;
using EventApp.Core.Services;
using EventApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.API.Controllers
{
    /// <summary>
    /// Controller for managing event types.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypeController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventTypeService _service;

        /// <summary>
        /// Initializes a new instance of the EventTypeController class.
        /// </summary>
        /// <param name="service">The service used to manage event types.</param>
        /// <param name="logger">This is for logging</param>
        public EventTypeController(IEventTypeService service, ILogger<EventsController> logger)
        {
            _service = service;
            _logger = logger;
        }


        /// <summary>
        /// This endpoint gets LIST of event types.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEventTypes()
        {
            try
            {
                var eventTypes = await _service.GetEventTypesAsync();

                if (eventTypes == null)
                {
                    return NotFound("No event types found.");
                }

                var eventTypeDtos = eventTypes.Select(e => new EventTypeDto
                {
                    EventTypeName = e.EventTypeName
                }).ToList();

                return Ok(eventTypeDtos);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                _logger.LogError(ex, "An error occurred while getting event types.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint gets DETAILS of an event type by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventType(int id)
        {
            try
            {
                var eventType = await _service.GetEventTypeByIdAsync(id);

                if (eventType == null)
                {
                    return NotFound("Event type not found.");
                }

                var eventTypeDto = new EventTypeDto
                {
                    EventTypeName = eventType.EventTypeName
                };

                return Ok(eventTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the event type.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint CREATES event type.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEventType(EventTypeDto eventTypeDto)
        {
            try
            {
                if (eventTypeDto == null)
                {
                    return BadRequest("EventTypeDto object is null");
                }

                var eventType = new EventType
                {
                    EventTypeName = eventTypeDto.EventTypeName
                };

                await _service.CreateEventTypeAsync(eventType);

                return CreatedAtAction(nameof(GetEventType), new { id = eventType.EventTypeId }, eventType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the event type.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint UPDATES/EDITS event type.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventType(int id, EventTypeDto eventTypeDto)
        {
            try
            {
                if (eventTypeDto == null)
                {
                    return BadRequest("EventTypeDto object is null");
                }

                var eventTypeEntity = await _service.GetEventTypeByIdAsync(id);
                if (eventTypeEntity == null)
                {
                    return NotFound();
                }

                eventTypeEntity.EventTypeName = eventTypeDto.EventTypeName;
                await _service.UpdateEventTypeAsync(eventTypeEntity);

                return Ok(eventTypeEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the event type.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint DELETES event type.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventType(int id)
        {
            try
            {
                var eventTypeToDelete = await _service.GetEventTypeByIdAsync(id);
                if (eventTypeToDelete == null)
                {
                    return NotFound($"Event type with id {id} not found.");
                }

                var eventTypeDto = new EventTypeDto
                {
                    EventTypeName = eventTypeToDelete.EventTypeName
                };

                await _service.DeleteEventTypeAsync(id);

                return Ok(eventTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the event type with id {id}.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }
}
