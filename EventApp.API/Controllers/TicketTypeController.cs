using EventApp.Core.DTO;
using EventApp.Core.Services;
using EventApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.API.Controllers
{
    /// <summary>
    /// Controller for managing ticket types.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ITicketTypeService _service;

        /// <summary>
        /// Initializes a new instance of the TicketTypeController class.
        /// </summary>
        /// <param name="service">The service used to manage ticket types.</param>
        /// <param name="logger">This is for logging</param>
        public TicketTypeController(ITicketTypeService service, ILogger<EventsController> logger)
        {
            _service = service;
            _logger = logger;
        }


        /// <summary>
        /// This endpoint gets LIST of ticket types.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTicketTypes()
        {
            try
            {
                var ticketTypes = await _service.GetTicketTypesAsync();

                if (ticketTypes == null || !ticketTypes.Any())
                {
                    return NotFound("No ticket type found.");
                }

                var ticketTypeDtos = ticketTypes.Select(t => new TicketTypeDto
                {
                    TicketTypeId = t.TicketTypeId,
                    TicketTypeName = t.TicketTypeName
                }).ToList();

                return Ok(ticketTypeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting ticket types.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint gets DETAILS of an ticket type by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketType(int id)
        {
            try
            {
                var ticketType = await _service.GetTicketTypeByIdAsync(id);

                if (ticketType == null)
                {
                    return NotFound("Ticket type not found.");
                }

                var ticketTypeDto = new TicketTypeDto
                {
                    TicketTypeName = ticketType.TicketTypeName
                };

                return Ok(ticketTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the ticket type.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint CREATES ticket type.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTicketType(TicketTypeDto ticketTypeDto)
        {
            try
            {
                if (ticketTypeDto == null)
                {
                    return BadRequest("TicketTypeDto object is null");
                }

                var ticketType = new TicketType
                {
                    TicketTypeName = ticketTypeDto.TicketTypeName
                };

                await _service.CreateTicketTypeAsync(ticketType);

                return CreatedAtAction(nameof(GetTicketType), new { id = ticketType.TicketTypeId }, ticketType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the ticket type.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint UPDATES/EDITS ticket type.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicketType(int id, TicketTypeDto ticketTypeDto)
        {
            try
            {
                var ticketType = await _service.GetTicketTypeByIdAsync(id);
                if (ticketType == null)
                {
                    return NotFound($"Ticket type with id {id} not found.");
                }

                ticketType.TicketTypeName = ticketTypeDto.TicketTypeName ?? ticketType.TicketTypeName;

                await _service.UpdateTicketTypeAsync(ticketType);

                return Ok(ticketType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the ticket type with id {id}.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        /// <summary>
        /// This endpoint DELETES ticket type.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketType(int id)
        {
            try
            {
                var ticketTypeToDelete = await _service.GetTicketTypeByIdAsync(id);
                if (ticketTypeToDelete == null)
                {
                    return NotFound($"Ticket type with id {id} not found.");
                }

                var ticketTypeDto = new TicketTypeDto
                {
                    TicketTypeId = ticketTypeToDelete.TicketTypeId,
                    TicketTypeName = ticketTypeToDelete.TicketTypeName
                };

                await _service.DeleteTicketTypeAsync(id);

                return Ok(ticketTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the ticket type with id {id}.");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
