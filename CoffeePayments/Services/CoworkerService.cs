using CoffeePaymentSystem.Data;
using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Services;

public class CoworkerService : ICoworkerService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CoworkerService> _logger;

    public CoworkerService(AppDbContext context, ILogger<CoworkerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<Coworker> GetCoworkers()
    {
        _logger.LogInformation("Retrieving all coworkers.");
        return _context.Coworkers.ToList();
    }

    public List<Coworker> GetActiveCoworkers()
    {
        _logger.LogInformation("Retrieving active coworkers.");
        return _context.Coworkers.Where(c => c.IsActive).ToList();
    }

    public void AddCoworker(Coworker newCoworker)
    {
        if (newCoworker == null)
        {
            throw new ArgumentNullException(nameof(newCoworker));
        }

        _logger.LogInformation("Adding new coworker: {Name}", newCoworker.Name);
        _context.Coworkers.Add(newCoworker);
        _context.SaveChanges();
    }

    public bool RemoveCoworker(int id)
    {
        var coworker = _context.Coworkers.Find(id);
        if (coworker == null)
        {
            _logger.LogWarning("Attempted to remove non-existent coworker with ID {Id}", id);
            return false;
        }

        coworker.IsActive = false;
        _context.SaveChanges();
        _logger.LogInformation("Coworker with ID {Id} marked as inactive.", id);
        return true;
    }

    public Coworker? GetCoworker(int id)
    {
        _logger.LogInformation("Retrieving coworker with ID {Id}", id);
        return _context.Coworkers.FirstOrDefault(c => c.Id == id);
    }
    
    
    public void UpdateBalancesOnPayment(int payerId)
    {
        _logger.LogInformation("Updating balances on payment. Payer ID: {PayerId}", payerId);

        var coworkers = GetActiveCoworkers();
        var totalDrinkCost = coworkers.Sum(c => c.DrinkCost);

        _logger.LogInformation("Total drink cost for all active coworkers: {TotalDrinkCost}", totalDrinkCost);

        foreach (var coworker in coworkers)
        {
            if (coworker.Id == payerId)
            {
                var amount = totalDrinkCost - coworker.DrinkCost;
                coworker.Balance += amount;
                _logger.LogInformation("Payer (ID: {Id}) balance increased by {Amount}. New balance: {Balance}", coworker.Id, amount, coworker.Balance);
            }
            else
            {
                coworker.Balance -= coworker.DrinkCost;
                _logger.LogInformation("Coworker (ID: {Id}) balance decreased by {Amount}. New balance: {Balance}", coworker.Id, coworker.DrinkCost, coworker.Balance);
            }
        }

        _context.SaveChanges();
        _logger.LogInformation("Balances updated and changes saved to the database.");
    }
}