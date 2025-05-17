namespace CoffeePaymentSystem.Models;

public class CoffeeIndexViewModel
{
    public Coworker? NextPayer { get; set; }
    public List<Coworker> Coworkers { get; set; }
    public List<PaymentHistoryEntry> History { get; set; }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public List<CoworkerPaymentSummaryViewModel> Summary { get; set; }
}