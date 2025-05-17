using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Services;

public interface IPaymentCalculator
{
    Coworker? GetNextPayer();

    List<CoworkerPaymentSummaryViewModel> GetFairnessSummary();
}