using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Services;

public class PaymentCalculator : IPaymentCalculator
{
    private readonly IPaymentHistoryService _paymentHistoryService;
    private readonly ICoworkerService _coworkerService;
    private readonly ILogger<PaymentCalculator> _logger;

    public PaymentCalculator(
        IPaymentHistoryService paymentHistoryService,
        ICoworkerService coworkerService,
        ILogger<PaymentCalculator> logger)
    {
        _paymentHistoryService = paymentHistoryService;
        _coworkerService = coworkerService;
        _logger = logger;
    }

    public Coworker? GetNextPayer()
    {
        _logger.LogInformation("Calculating next payer.");
        var paymentHistory = _paymentHistoryService.GetPaymentHistory();
        var coworkers = _coworkerService.GetActiveCoworkers();
        var payments = paymentHistory.Where(p => !p.IsSkip).ToList();

        var lastPaymentDate = payments.Any()
            ? payments.Max(p => p.DatePaid)
            : DateTime.MinValue;

        var activeSkips = paymentHistory
            .Where(p => p.IsSkip && p.DatePaid > lastPaymentDate)
            .Select(p => p.CoworkerId)
            .ToHashSet();

        var balances = coworkers
            .Where(c => !activeSkips.Contains(c.Id))
            .Select(c => new
            {
                Coworker = c,
            });

        var nextPayer = balances.OrderBy(b => b.Coworker.Balance)
            .ThenByDescending(b => b.Coworker.DrinkCost)
            .FirstOrDefault()?.Coworker;

        _logger.LogInformation("Next payer determined: {Name}", nextPayer?.Name);
        return nextPayer;
    }

    public List<CoworkerPaymentSummaryViewModel> GetFairnessSummary()
    {
        _logger.LogInformation("Generating fairness summary.");
        var coworkers = _coworkerService.GetActiveCoworkers();

        var paymentSummary = coworkers.Select(c =>
        {
            return new CoworkerPaymentSummaryViewModel
            {
                Name = c.Name,
                FavoriteDrink = c.FavoriteDrink,
                DrinkCost = c.DrinkCost,
                Balance = c.Balance
            };
        }).ToList();

        return paymentSummary;
    }

   
}