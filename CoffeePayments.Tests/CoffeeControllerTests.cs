using CoffeePaymentSystem.Controllers;
using CoffeePaymentSystem.Models;
using CoffeePaymentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoffeePayments.Tests
{
    public class CoffeeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewWithCorrectViewModel()
        {
            // Arrange
            var coworkers = new List<Coworker>
            {
                new Coworker { Id = 1, Name = "Alice", IsActive = true },
                new Coworker { Id = 2, Name = "Bob", IsActive = true }
            };
            var paymentHistory = new List<PaymentHistoryEntry>
            {
                new PaymentHistoryEntry { CoworkerId = 1, DatePaid = DateTime.Today },
                new PaymentHistoryEntry { CoworkerId = 2, DatePaid = DateTime.Today.AddDays(-1) }
            };
            var summary = new List<CoworkerPaymentSummaryViewModel>
            {
                new CoworkerPaymentSummaryViewModel { Name = "Alice" }
            };
            var nextPayer = new Coworker { Id = 1, Name = "Alice" };

            var paymentCalculator = new Mock<IPaymentCalculator>();
            paymentCalculator.Setup(x => x.GetFairnessSummary()).Returns(summary);
            paymentCalculator.Setup(x => x.GetNextPayer()).Returns(nextPayer);

            var paymentHistoryService = new Mock<IPaymentHistoryService>();
            paymentHistoryService.Setup(x => x.GetPaymentHistory()).Returns(paymentHistory);

            var coworkerService = new Mock<ICoworkerService>();
            coworkerService.Setup(x => x.GetCoworkers()).Returns(coworkers);


            var controller = new CoffeeController(
                paymentCalculator.Object,
                paymentHistoryService.Object,
                coworkerService.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CoffeeIndexViewModel>(viewResult.Model);
            Assert.Equal(2, model.Coworkers.Count);
            Assert.Equal(2, model.History.Count);
            Assert.Equal(summary, model.Summary);
            Assert.Equal(nextPayer, model.NextPayer);
        }

        [Fact]
        public void AddPayment_ValidCoworker_CallsAddPaymentAndRedirects()
        {
            // Arrange
            var coworker = new Coworker { Id = 1, Name = "Alice" };
            var paymentCalculator = new Mock<IPaymentCalculator>();
            var paymentHistoryService = new Mock<IPaymentHistoryService>();
            var coworkerService = new Mock<ICoworkerService>();
            coworkerService.Setup(x => x.GetCoworker(1)).Returns(coworker);

            var controller = new CoffeeController(
                paymentCalculator.Object,
                paymentHistoryService.Object,
                coworkerService.Object);

            // Act
            var result = controller.AddPayment(1);

            // Assert
            paymentHistoryService.Verify(x => x.AddPayment(1), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public void AddPayment_InvalidCoworker_DoesNotCallAddPayment()
        {
            // Arrange
            var paymentCalculator = new Mock<IPaymentCalculator>();
            var paymentHistoryService = new Mock<IPaymentHistoryService>();
            var coworkerService = new Mock<ICoworkerService>();
            coworkerService.Setup(x => x.GetCoworker(1)).Returns((Coworker)null);

            var controller = new CoffeeController(
                paymentCalculator.Object,
                paymentHistoryService.Object,
                coworkerService.Object);

            // Act
            var result = controller.AddPayment(1);

            // Assert
            paymentHistoryService.Verify(x => x.AddPayment(It.IsAny<int>()), Times.Never);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public void SkipPerson_CallsAddSkippedAndRedirects()
        {
            // Arrange
            var paymentCalculator = new Mock<IPaymentCalculator>();
            var paymentHistoryService = new Mock<IPaymentHistoryService>();
            var coworkerService = new Mock<ICoworkerService>();

            var controller = new CoffeeController(
                paymentCalculator.Object,
                paymentHistoryService.Object,
                coworkerService.Object);

            // Act
            var result = controller.SkipPerson(2);

            // Assert
            paymentHistoryService.Verify(x => x.AddSkipped(2), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}