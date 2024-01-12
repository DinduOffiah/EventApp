using EventApp.API.Controllers;
using EventApp.Core.DTO;
using EventApp.Core.Services;
using EventApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventApp.Tests;
public class EventsControllerTests
{
    private readonly Mock<IEventService> _mockService;
    private readonly Mock<ILogger<EventsController>> _mockLogger;
    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _mockService = new Mock<IEventService>();
        _mockLogger = new Mock<ILogger<EventsController>>();
        _controller = new EventsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetEvent_ReturnsNotFound_WhenEventDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync((Event?)null);

        // Act
        var result = await _controller.GetEvent(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    //Create Event
    [Fact]
    public async Task PostEvent_ReturnsCreatedAtAction_WhenEventIsCreated()
    {
        // Arrange
        var eventDate = DateTime.UtcNow;

        var newEvent = new EventDto
        {
            EventName = "Test Event",
            Image = new byte[] { 0, 1, 2, 3, 4, 5 },
            EventDate = eventDate,
            Location = "Test Location",
            TicketTypeId = 1,
            EventTypeId = 1,
            Limit = 100
        };

        var createdEvent = new Event
        {
            EventName = "Test Event",
            Image = new byte[] { 0, 1, 2, 3, 4, 5 },
            EventDate = eventDate,
            Location = "Test Location",
            TicketTypeId = 1,
            EventTypeId = 1,
            Limit = 100
        };

        _mockService.Setup(service => service.CreateEventAsync(It.IsAny<Event>())).ReturnsAsync(createdEvent);

        // Act
        var result = await _controller.PostEvent(newEvent);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<EventDto>(createdAtActionResult.Value);
        Assert.Equal(newEvent.EventName, returnValue.EventName);
        Assert.Equal(newEvent.EventDate, returnValue.EventDate);
        Assert.Equal(newEvent.Image, returnValue.Image);
        Assert.Equal(newEvent.Location, returnValue.Location);
        Assert.Equal(newEvent.Limit, returnValue.Limit);
        Assert.Equal(newEvent.EventTypeId, returnValue.EventTypeId);
        Assert.Equal(newEvent.TicketTypeId, returnValue.TicketTypeId);
    }


    //Update Event
    [Fact]
    public async Task PutEvent_ReturnsOk_WhenEventIsUpdated()
    {
        // Arrange
        var existingEvent = new Event { /* set properties here */ };
        var updatedEvent = new EventDto { /* set properties here */ };

        _mockService.Setup(service => service.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync(existingEvent);

        // Act
        var result = await _controller.PutEvent(1, updatedEvent);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Event>(okResult.Value);
        // You can add more assertions here to check the properties of the returned event
    }


    //Delete Event
    [Fact]
    public async Task DeleteEvent_ReturnsOk_WhenEventIsDeleted()
    {
        // Arrange
        var eventToDelete = new Event
        {
            EventName = "Test Event",
            Image = new byte[] { 0, 1, 2, 3, 4, 5 },
            EventDate = DateTime.Now,
            Location = "Test Location",
            TicketTypeId = 1,
            EventTypeId = 1,
            Limit = 100
        };


        _mockService.Setup(service => service.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync(eventToDelete);

        // Act
        var result = await _controller.DeleteEvent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EventDto>(okResult.Value);
        Assert.Equal(eventToDelete.EventName, returnValue.EventName);
        Assert.Equal(eventToDelete.EventDate, returnValue.EventDate);
        Assert.Equal(eventToDelete.Image, returnValue.Image);
        Assert.Equal(eventToDelete.Location, returnValue.Location);
        Assert.Equal(eventToDelete.Limit, returnValue.Limit);
        Assert.Equal(eventToDelete.EventTypeId, returnValue.EventTypeId);
        Assert.Equal(eventToDelete.TicketTypeId, returnValue.TicketTypeId);
    }



}
