using CoffeePaymentSystem.Data;
using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Services;

public class PaymentHistoryService : IPaymentHistoryService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PaymentHistoryService> _logger;

    public PaymentHistoryService(AppDbContext dbContext, ILogger<PaymentHistoryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void AddPayment(int coworkerId)
    {
        if (!_dbContext.Coworkers.Any(c => c.Id == coworkerId))
        {
            throw new ArgumentException($"Coworker with ID {coworkerId} does not exist.", nameof(coworkerId));
        }

        var coworkers = _dbContext.Coworkers.ToList();
        var totalCost = coworkers.Sum(c => c.DrinkCost);

        var paymentEntry = new PaymentHistoryEntry
        {
            CoworkerId = coworkerId,
            DatePaid = DateTime.Now,
            AmountPaid = totalCost
        };

        _dbContext.PaymentHistory.Add(paymentEntry);
        _dbContext.SaveChanges();

        _logger.LogInformation("Added payment for coworker ID {CoworkerId} with total cost {TotalCost}.", coworkerId, totalCost);
    }

    public void AddSkipped(int coworkerId)
    {
        if (!_dbContext.Coworkers.Any(c => c.Id == coworkerId))
        {
            throw new ArgumentException($"Coworker with ID {coworkerId} does not exist.", nameof(coworkerId));
        }

        var paymentEntry = new PaymentHistoryEntry
        {
            CoworkerId = coworkerId,
            DatePaid = DateTime.Now,
            AmountPaid = 0,
            IsSkip = true
        };

        _dbContext.PaymentHistory.Add(paymentEntry);
        _dbContext.SaveChanges();

        _logger.LogInformation("Added skipped payment for coworker ID {CoworkerId}.", coworkerId);
    }

    public List<PaymentHistoryEntry> GetPaymentHistory()
    {
        var history = _dbContext.PaymentHistory
            .OrderByDescending(p => p.DatePaid)
            .ToList();

        _logger.LogInformation("Retrieved {Count} payment history entries.", history.Count);

        return history;
    }
}