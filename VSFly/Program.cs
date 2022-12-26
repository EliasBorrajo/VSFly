using VSFly;
using VSFly.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;

Console.WriteLine("VS Fly Core APP");
using (var context = new VSFlyContext())
{
    // 1) Tester si la DB existe déja ou si il faut la créer
    Boolean DbCreated = context.Database.EnsureCreated();

    if (DbCreated)
        Console.WriteLine("Database has been created");
    else
        Console.WriteLine("Database already exists");

    Console.WriteLine("Done");

    // 2) Seed la DB si elle est vide
    // Retourne "true" si la table possède des entrées
    bool isEmpty = context.Passengers.Any() &&
                             context.Employees.Any() &&
                             context.Bookings.Any() &&
                             context.Flights.Any();
    if ( ! isEmpty )
    {
        Console.WriteLine("The database is empty. Seed it.");
        SeedDB(context);

    }
    else
    {
        Console.WriteLine("The database is not empty. No need to seed it.");
    }

    PrintFlightsAvailable(context);
    
    Console.WriteLine("\nProgramme terminé.");

}

void PrintFlightsAvailable(VSFlyContext context)
{
    Console.WriteLine("******************************************************************");
    Console.WriteLine("\nAffichage des vols libres :");

    var volsLibres = context.Flights.Where(i => i.FreeSeats > 0).ToList();

    foreach (var vol in volsLibres)
    {
        Console.WriteLine("******************************************************************");
        Console.WriteLine(  "Vol N°" + vol.FlightNo + 
                                        "\nDépart : " + vol.Departure + 
                                        "\nDestination : " + vol.Destination + 
                                        "\nDate du vol : " + vol.Date +
                                         "\nMois entre le départ et aujourd'hui : " + NbrMonthsBeforeDeparture(vol.Date, DateTime.Now) +
                                        "\nSièges libres : " + vol.FreeSeats + 
                                        "\n% sièges remplis : " + CalculateFillingseats(vol) + 
                                        "\nPrix de base du ticket : " +vol.BasePrice + " CHF"+ 
                                        "\nPrix calculé du ticket : " + CalculateSalePrice(vol) + " CHF\n");
    }
    Console.WriteLine("******************************************************************");

}

void SeedDB(VSFlyContext context)
{
    Console.WriteLine("Seeding the DB");
                                                                                                                             // Date Time  = Year, Month, Day, Hour, Minutes, Seconds
    var flight1 = new Flight() { Departure = "Genève", Destination = "Tokyo",    Date = new DateTime(2023, 1, 22, 12,   0, 0),   BasePrice = 60.00, TotalSeats = 100, FreeSeats = 81 };
    var flight2 = new Flight() { Departure = "Sion",      Destination = "Londres", Date = new DateTime(2023, 1,  1, 18, 30, 0),   BasePrice = 80.00, TotalSeats = 200, FreeSeats = 101 };

    var passenger1 = new Passenger() { Name = "Gandalf", isActiveClient = true, Email = "gandalf@gmail.com" };
    var passenger2 = new Passenger() { Name = "Saruman", isActiveClient = true, Email = "saruman@gmail.com" };
    var passenger3 = new Passenger() { Name = "Radagast", isActiveClient = true, Email = "radagast@gmail.com" };

    // flight 1 avec employée 1 & pilot 1
    var pilot1 = new Employee()          { Name = "Arthur", isPilot = true, Email = "arthur@gmail.com", Salary = 5000.00 , Flight = flight1};
    var employee1 = new Employee() { Name = "Merlin", isPilot = false, Email = "merlin@gmail.com", Salary = 3000.00,  Flight = flight1 };

    // flight 2 avec employée 2 & pilot 2
    var pilot2 = new Employee()          { Name = "Perceval",  isPilot = true, Email = "perceval@gmail.com", Salary = 10000.00, Flight = flight2 };
    var employee2 = new Employee() { Name = "Karadock", isPilot = false, Email = "karadock@gmail.com", Salary = 6000.00, Flight = flight2 };

    // Le booking est lié au passenger, et détient les informations d'achats de SON vol
    // Vol 1 - Passager 1
    Booking booking1 = new Booking() { Flight = flight1, Passenger = passenger1 , SalePrice = CalculateSalePrice(flight1) };
    flight1.FreeSeats--;

    // Vol 1 - Passager 2
    Booking booking2 = new Booking() { Flight = flight1, Passenger = passenger2, SalePrice = CalculateSalePrice(flight1) };
    flight1.FreeSeats--;

    // Vol 2 - Passager 2
    Booking booking3 = new Booking() { Flight = flight2, Passenger = passenger2, SalePrice = CalculateSalePrice(flight2) };
    flight2.FreeSeats--;

    // Vol 2 - Passager 3
    Booking booking4 = new Booking() { Flight = flight2, Passenger = passenger3, SalePrice = CalculateSalePrice(flight2) };
    flight2.FreeSeats--;

    if(true)
    {
        Console.WriteLine("Adding to the context");
        context.Flights.Add(flight1);
        context.Flights.Add(flight2);

        context.Passengers.Add(passenger1);
        context.Passengers.Add(passenger1);
        context.Passengers.Add(passenger1);

        context.Employees.Add(employee1);
        context.Employees.Add(employee2);
        context.Employees.Add(pilot1);
        context.Employees.Add(pilot2);

        context.Bookings.Add(booking1);
        context.Bookings.Add(booking2);
        context.Bookings.Add(booking3);
        context.Bookings.Add(booking4);

        Console.WriteLine("Save Changes to DB");
        context.SaveChanges(); // Tant que on apelle pas ça, la DB n'est pas changé

        Console.WriteLine("SEEDING DONE");
    }


}

/// <summary>
/// Calculates the percentage of seats that are filled on a flight.
/// </summary>
/// <param name="flight">The flight for which to calculate the filling percentage.</param>
/// <returns>The percentage of seats that are filled on the flight.</returns>
double CalculateFillingseats(Flight flight)
{
    return  ((double)flight.TotalSeats - (double)flight.FreeSeats) / (double)flight.TotalSeats * 100.00;
}

/// <summary>
/// Calculates the sale price for a flight based on the filling percentage of the seats and the number of months before the departure date.
/// </summary>
/// <param name="flight">The flight for which to calculate the sale price.</param>
/// <returns>The sale price for the flight.</returns>
double CalculateSalePrice(Flight flight)
{
    // % seats fillled = (total - free) / total * 100
    double FillingSeatsPercent = CalculateFillingseats(flight);
    //Console.WriteLine("Filling Seats % = " + FillingSeatsPercent);
     
    if(FillingSeatsPercent >= 80)
    {
        //Console.WriteLine("1.\tIf the airplane is more than 80% full regardless of the date. \ta. sale price = 150% of the base price\r\n");
        return flight.BasePrice * 1.5;
    }
    else if ( FillingSeatsPercent < 20 && 
                NbrMonthsBeforeDeparture(flight.Date, DateTime.Now) < 2)
    {
        //Console.WriteLine("2.\tIf the plane is filled less than 20% less than 2 months before departure: \ta. sale price = 80% of the base price\r\n");
        return flight.BasePrice * 0.8;
    }
    else if ( FillingSeatsPercent < 50 &&
                NbrMonthsBeforeDeparture(flight.Date, DateTime.Now) < 1)
    {
        //Console.WriteLine("3.\tIf the plane is filled less than 50% less than 1 month before departure: \ta. sale price = 70% of the base price\r\n");
        return flight.BasePrice * 0.7;
    }
    else
    {
        //Console.WriteLine("4.\tIn all other cases: \ta. sale price = base price\r\n");
        return flight.BasePrice;
    }
}

/// <summary>
/// method first checks if the years of the two dates are the same. 
/// If they are, the method calculates the absolute difference between the months in the same way as before. 
/// If the years are different, the method calculates the number of months remaining in the first year 
/// and adds 12 months for each additional year between the two dates
/// </summary>
int NbrMonthsBeforeDeparture(DateTime FlightDate, DateTime ActualDate)
{
    int monthsbetween = 0;

    if (FlightDate.Year == ActualDate.Year)
    {
        monthsbetween = Math.Abs(FlightDate.Month - ActualDate.Month);
    }
    else
    {
        int yearsBetween = Math.Abs(FlightDate.Year - ActualDate.Year);
        int monthsInFirstYear = 12 - ActualDate.Month;
        monthsbetween = monthsInFirstYear;

        for (int i = 1; i < yearsBetween; i++)
        {
            monthsbetween += 12;
        }

    }

    //Console.WriteLine("Months between departure : " + monthsbetween);
    return monthsbetween;


   
}


