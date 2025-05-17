using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Services;

public interface IPaymentHistoryService
{
    List<PaymentHistoryEntry> GetPaymentHistory();
    void AddPayment(int coworkerId);
    void AddSkipped(int coworkerId);
}