using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeePaymentSystem.Models;

public class PaymentHistoryEntry
{
    public int Id { get; set; }

    [ForeignKey("Coworker")]
    public int CoworkerId { get; set; }
    [Required]
    public Coworker Payer { get; set; }
    [Required]
    public DateTime DatePaid { get; set; }
    [Required]
    public decimal AmountPaid { get; set; }
    public bool IsSkip { get; set; }
}