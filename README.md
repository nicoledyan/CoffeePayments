# CoffeePayments
A simple web application to track coffee payments and fairness among coworkers. 
The fairness logic uses a running balance for each coworker:

- When someone pays for a round, their balance increases by the total cost of all drinks minus their own drink cost.
- Each other active coworker's balance decreases by the cost of their own drink.
- This means the payer is credited for covering everyone else, and each participant is debited for their share.
- Over time, the balances reflect who has paid more or less than their fair share. The next payer is the coworker with the lowest balance.

## Features

- Track payments and skipped turns for each coworker  
- Calculate fairness and balances  
- Add, remove, and manage coworkers  
- View payment history and next payer

## Tech Stack

- ASP.NET Core MVC (C#)  
- Entity Framework Core (EF Core)  
- SQLite
- JavaScript (for UI enhancements)
- HTML & CSS

# Getting Started

## Prerequisites

- .NET 8 SDK  
- SQLite 
- Visual Studio or JetBrains Rider

## Setup

1. Clone the repository:  
   git clone https://github.com/nicoledyan/CoffeePayments.git
   cd CoffeePayments

3. Configure the database:  
   Update the connection string in appsettings.json to point to your SQL Server or SQLite instance.

4. Apply migrations:  
   dotnet ef database update

5. Seed the database:  
   The application seeds initial data automatically on first run using the DbInitializer in CoffeePayments/Data/DbInitializer.cs.  
   The seed includes sample coworkers and payment history(currently commented out).

6. Run the application:  
   dotnet run  
   Visit http://localhost:5237 in your browser.

## Database Seeding

- On first run, if the database is empty, the DbInitializer.Seed method will populate it with:  
  - Sample coworkers (names, favorite drinks, join dates)  
  - Sample payment history entries (this seeding is currently commented out)  
- To clear and reseed the database, you can call the DbInitializer.Clear method (db is cleared with each run of the project currently via Program.cs).

# Assumptions and future considerations

- The drink type and price for each coworker are assumed to remain constant after being set.  
- Adding functionality to update drink names and prices should be concidered for the future
    - would we want to keep track of what drinks are ordered?
    - make sure running balance uses the correct drink and price to update
 - Database can be switched out for a SQL database instead of using SQLite
 - Logging can be switched to use a logging service like App Insights instead of console logging

Project Structure

- Controllers/ — MVC controllers  
- Models/ — Data models (Coworker, PaymentHistoryEntry, etc.)  
- Services/ — Business logic and calculation services  
- Data/ — Database context and seeding logic  
- Views/ — Razor views for UI

