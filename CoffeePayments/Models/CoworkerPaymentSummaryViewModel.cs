namespace CoffeePaymentSystem.Models;

public class CoworkerPaymentSummaryViewModel
{
    public string Name { get; set; }
    public string FavoriteDrink { get; set; }
    public decimal DrinkCost { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalOwed { get; set; }
    public decimal Balance {get; set;}
}
