using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Services;

public interface ICoworkerService
{
    List<Coworker> GetCoworkers();
    void AddCoworker(Coworker newCoworker);
    bool RemoveCoworker(int id);
    List<Coworker> GetActiveCoworkers();
    Coworker? GetCoworker(int coworkerId);
    void UpdateBalancesOnPayment(int payerId);
}