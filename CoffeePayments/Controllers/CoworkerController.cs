using CoffeePaymentSystem.Models;
using CoffeePaymentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeePaymentSystem.Controllers;

[Route("coworker")]
public class CoworkerController : Controller
{
    private readonly ICoworkerService _coworkerService;
    public CoworkerController(
        ICoworkerService coworkerService)
    {
        _coworkerService = coworkerService;
    }
    [HttpPost("AddCoworker")]
    public IActionResult AddCoworker(string name, string favoriteDrink, decimal drinkCost, DateTime joinDate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid model state");
        }

        var newCoworker = new Coworker
        {
            Name = name,
            FavoriteDrink = favoriteDrink,
            DrinkCost = drinkCost,
            DateJoined = joinDate
        };

        _coworkerService.AddCoworker(newCoworker);

        var updatedCoworkers = _coworkerService.GetActiveCoworkers(); 
        return PartialView("_CoworkerListPartial", updatedCoworkers);
    }
        
    [HttpPost("RemoveCoworker")]
    public IActionResult RemoveCoworker(int id)
    {
        var success = _coworkerService.RemoveCoworker(id);
        if (!success) return NotFound();

        var updatedCoworkers = _coworkerService.GetActiveCoworkers(); 
        return PartialView("_CoworkerListPartial", updatedCoworkers);
    }
}