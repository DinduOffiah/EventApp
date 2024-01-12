using EventApp.API.Controllers;
using EventApp.Core.DTO;
using EventApp.Core.Services;
using EventApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventApp.Tests
{
    public class TicketsTypeControllerTests
    {
        private Mock<ITicketTypeService> _service;
        private Mock<ILogger<EventsController>> _logger;
        private TicketTypeController _controller;

        public TicketsTypeControllerTests()
        {
            _service = new Mock<ITicketTypeService>();
            _logger = new Mock<ILogger<EventsController>>();
            _controller = new TicketTypeController(_service.Object, _logger.Object);
        }

        [Fact]
        public async Task GetTicketTypes_ReturnsNotFound_WhenNoTicketTypesExist()
        {
            _service.Setup(s => s.GetTicketTypesAsync()).ReturnsAsync((List<TicketType>)null);

            var result = await _controller.GetTicketTypes();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        //Create Event
        [Fact]
        public async Task CreateTicketType_ReturnsBadRequest_WhenTicketTypeDtoIsNull()
        {
            var result = await _controller.CreateTicketType(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Update Event
        [Fact]
        public async Task UpdateTicketType_ReturnsNotFound_WhenTicketTypeDoesNotExist()
        {
            _service.Setup(s => s.GetTicketTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((TicketType)null);

            var result = await _controller.UpdateTicketType(1, new TicketTypeDto());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        //Delete Event
        [Fact]
        public async Task DeleteTicketType_ReturnsNotFound_WhenTicketTypeDoesNotExist()
        {
            _service.Setup(s => s.GetTicketTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((TicketType)null);

            var result = await _controller.DeleteTicketType(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

}
