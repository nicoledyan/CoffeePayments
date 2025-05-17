using CoffeePaymentSystem.Services;
using CoffeePaymentSystem.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeePayments.Tests;

public class PaymentCalculatorTests
{
    [Fact]
    public void GetNextPayer_NoPayments_AllCoworkersConsidered()
    {
        // Arrange
        var coworkers = new List<Coworker>
        {
            new Coworker { Name = "Alice", DrinkCost = 3, IsActive = true, DateJoined = DateTime.Today.AddDays(-3)  },
            new Coworker { Name = "Bob", DrinkCost = 5, IsActive = true, DateJoined = DateTime.Today.AddDays(-2)  },
            new Coworker { Name = "Charlie", DrinkCost = 2, IsActive = true, DateJoined = DateTime.Today.AddDays(-2)  }
        };

        var payments = new List<PaymentHistoryEntry>(); // no payments

        var mockPaymentHistoryService = new Mock<IPaymentHistoryService>();
        mockPaymentHistoryService.Setup(s => s.GetPaymentHistory()).Returns(payments);
        var mockCoworkerService = new Mock<ICoworkerService>();
        mockCoworkerService.Setup(s => s.GetActiveCoworkers()).Returns(coworkers);
        var mockLogger = new Mock<ILogger<PaymentCalculator>>();
        
        var fairCostPaymentCalculator = new PaymentCalculator(
            mockPaymentHistoryService.Object, 
            mockCoworkerService.Object,
            mockLogger.Object);

        // Act
        var nextPayer = fairCostPaymentCalculator.GetNextPayer();

        // Assert
        Assert.Equal("Bob", nextPayer?.Name);
    }

    [Fact]
    public void GetNextPayer_ReturnsCoworkerWithLowestBalance_ExcludingActiveSkips()
    {
        // Arrange
        
        
        var coworkers = new List<Coworker>
        {
            new Coworker { Id = 1, Name = "Alice", DrinkCost = 2, IsActive = true, DateJoined = DateTime.Today.AddDays(-3) },
            new Coworker { Id = 2, Name = "Bob", DrinkCost = 5, IsActive = true, DateJoined = DateTime.Today.AddDays(-2) },
            new Coworker { Id = 3, Name = "Charlie", DrinkCost = 6, IsActive = true, DateJoined = DateTime.Today.AddDays(-1) }
        };

        var payments = new List<PaymentHistoryEntry>
        {
            new PaymentHistoryEntry { CoworkerId = 1, AmountPaid = 15, DatePaid = DateTime.Today.AddDays(-2), IsSkip = false },
            new PaymentHistoryEntry { CoworkerId = 2, AmountPaid = 10, DatePaid = DateTime.Today.AddDays(-1), IsSkip = false },
            new PaymentHistoryEntry { CoworkerId = 3, AmountPaid = 0, DatePaid = DateTime.Today, IsSkip = true }
        };

        var mockPaymentHistoryService = new Mock<IPaymentHistoryService>();
        mockPaymentHistoryService.Setup(s => s.GetPaymentHistory()).Returns(payments);
        var mockCoworkerService = new Mock<ICoworkerService>();
        mockCoworkerService.Setup(s => s.GetActiveCoworkers()).Returns(coworkers);
        var mockLogger = new Mock<ILogger<PaymentCalculator>>();
        
        var fairCostPaymentCalculator = new PaymentCalculator(
            mockPaymentHistoryService.Object, 
            mockCoworkerService.Object,
            mockLogger.Object);
        
        // Act
        var nextPayer = fairCostPaymentCalculator.GetNextPayer();

        // Assert
        Assert.Equal("Bob", nextPayer?.Name);
    }
}