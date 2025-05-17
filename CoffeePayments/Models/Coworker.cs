using System.ComponentModel.DataAnnotations;

namespace CoffeePaymentSystem.Models;

public class Coworker
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(100)]
    public string FavoriteDrink { get; set; }
    [Required]
    public decimal DrinkCost { get; set; }
    [Required]
    public DateTime DateJoined { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal Balance { get; set; }
}
