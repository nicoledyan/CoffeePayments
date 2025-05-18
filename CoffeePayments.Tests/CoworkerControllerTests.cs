using CoffeePaymentSystem.Controllers;
using CoffeePaymentSystem.Models;
using CoffeePaymentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoffeePayments.Tests
{
    public class CoworkerControllerTests
    {

        [Fact]
        public void AddCoworker_ValidModel_AddsCoworkerAndRedirectsToPartial()
        {
            // Arrange
            var coworkerService = new Mock<ICoworkerService>();

            var controller = new CoworkerController(
                coworkerService.Object);

            controller.ModelState.Clear();

            // Act
            var result = controller.AddCoworker("Alice", "Latte", 3.5m, DateTime.Today);

            // Assert
            coworkerService.Verify(x => x.AddCoworker(It.Is<Coworker>(c =>
                c.Name == "Alice" &&
                c.FavoriteDrink == "Latte" &&
                c.DrinkCost == 3.5m &&
                c.DateJoined == DateTime.Today)), Times.Once);

            var redirect = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_CoworkerListPartial", redirect.ViewName);}

        [Fact]
        public void AddCoworker_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var coworkerService = new Mock<ICoworkerService>();

            var controller = new CoworkerController(
                coworkerService.Object);

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.AddCoworker("", "", 0, DateTime.Today);

            // Assert
            coworkerService.Verify(x => x.AddCoworker(It.IsAny<Coworker>()), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void RemoveCoworker_Successful_RemovesAndRedirectsToPartial()
        {
            // Arrange
            var coworkerService = new Mock<ICoworkerService>();
            coworkerService.Setup(x => x.RemoveCoworker(1)).Returns(true);

            var controller = new CoworkerController(
                coworkerService.Object);

            // Act
            var result = controller.RemoveCoworker(1);

            // Assert
            coworkerService.Verify(x => x.RemoveCoworker(1), Times.Once);
            var redirect = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_CoworkerListPartial", redirect.ViewName);
        }

        [Fact]
        public void RemoveCoworker_NotFound_ReturnsNotFound()
        {
            // Arrange
            var coworkerService = new Mock<ICoworkerService>();
            coworkerService.Setup(x => x.RemoveCoworker(1)).Returns(false);

            var controller = new CoworkerController(
                coworkerService.Object);

            // Act
            var result = controller.RemoveCoworker(1);

            // Assert
            coworkerService.Verify(x => x.RemoveCoworker(1), Times.Once);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}