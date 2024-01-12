using EventApp.API.Controllers;
using EventApp.Core.DTO;
using EventApp.Core.Services;
using EventApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventApp.Tests
{
    public class EventTypeControllerTests
    {
        private Mock<IEventTypeService> _service;
        private Mock<ILogger<EventsController>> _logger;
        private EventTypeController _controller;

        public EventTypeControllerTests()
        {
            _service = new Mock<IEventTypeService>();
            _logger = new Mock<ILogger<EventsController>>();
            _controller = new EventTypeController(_service.Object, _logger.Object);
        }

        [Fact]
        public async Task GetEventType_ReturnsNotFound_WhenEventTypeDoesNotExist()
        {
            _service.Setup(s => s.GetEventTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((EventType)null);

            var result = await _controller.GetEventType(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        //Create Event
        [Fact]
        public async Task CreateEventType_ReturnsBadRequest_WhenEventTypeDtoIsNull()
        {
            var result = await _controller.CreateEventType(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        //Update Event
        [Fact]
        public async Task UpdateEventType_ReturnsNotFound_WhenEventTypeDoesNotExist()
        {
            _service.Setup(s => s.GetEventTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((EventType)null);

            var result = await _controller.UpdateEventType(1, new EventTypeDto());

            Assert.IsType<NotFoundResult>(result);
        }

        //Delete Event
        [Fact]
        public async Task DeleteEventType_ReturnsNotFound_WhenEventTypeDoesNotExist()
        {
            _service.Setup(s => s.GetEventTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((EventType)null);

            var result = await _controller.DeleteEventType(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

    }

}
