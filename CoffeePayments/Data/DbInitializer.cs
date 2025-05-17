// File: Data/DbInitializer.cs

using CoffeePaymentSystem.Models;

namespace CoffeePaymentSystem.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Coworkers.Any())
        {
            context.Coworkers.AddRange(
                new Coworker { Id = 1, Name = "Bob", FavoriteDrink = "Cappuccino", DrinkCost = 4.50m , DateJoined = DateTime.Now.AddMonths(-4)},
                new Coworker { Id = 2, Name = "Nina", FavoriteDrink = "Mocha", DrinkCost = 7.00m , DateJoined = DateTime.Now.AddMonths(-3)},
                new Coworker { Id = 3, Name = "Alice", FavoriteDrink = "Latte", DrinkCost = 4.25m , DateJoined = DateTime.Now.AddMonths(-2)},
                new Coworker { Id = 4, Name = "Charlie", FavoriteDrink = "Macchiato", DrinkCost = 4.00m , DateJoined = DateTime.Now.AddDays(-16)},
                new Coworker { Id = 5, Name = "Jeremy", FavoriteDrink = "Black Coffee", DrinkCost = 3.00m , DateJoined = DateTime.Now.AddDays(-11)},
                new Coworker { Id = 6, Name = "Dan", FavoriteDrink = "Espresso", DrinkCost = 2.75m , DateJoined = DateTime.Now.AddDays(-1)},
                new Coworker { Id = 7, Name = "Eva", FavoriteDrink = "Iced Coffee", DrinkCost = 1.50m , DateJoined = DateTime.Now.AddDays(-1)}
            );
            context.SaveChanges();
        }
            /*
            if (!context.PaymentHistory.Any())
            {
                context.PaymentHistory.AddRange(
                    new PaymentHistoryEntry { Id = 1, CoworkerId = 1, DatePaid = DateTime.Now.AddMonths(-3), AmountPaid = 27.00m },
                    new PaymentHistoryEntry { Id = 2, CoworkerId = 2, DatePaid = DateTime.Now.AddMonths(-2), AmountPaid = 27.00m },
                    new PaymentHistoryEntry { Id = 3, CoworkerId = 3, DatePaid = DateTime.Now.AddMonths(-1), AmountPaid = 27.00m },
                    new PaymentHistoryEntry { Id = 4, CoworkerId = 4, DatePaid = DateTime.Now.AddDays(-15), AmountPaid = 27.00m },
                    new PaymentHistoryEntry { Id = 5, CoworkerId = 5, DatePaid = DateTime.Now.AddDays(-10), AmountPaid = 27.00m }
                );

                context.SaveChanges();
            }
            */
   
    }
    
    public static void Clear(AppDbContext context)
    {
        context.PaymentHistory.RemoveRange(context.PaymentHistory);
        context.Coworkers.RemoveRange(context.Coworkers);
        context.SaveChanges();
    }
}