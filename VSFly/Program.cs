using VSFly;
using VSFly.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using (var ctx = new AeroportContext())
{
    var DbCreated = ctx.Database.EnsureCreated();

    if (DbCreated)
    {
        Console.WriteLine("Création de la base de données...\n");
    } 
    else
    {
        Console.WriteLine("Base de données déjà créée.\n");
    }

    if (false)
    {
        Console.WriteLine("Insertion d'éléments dans la base de données...");

        var passager1 = new Passenger() { Nom = "Salamin", Prenom = "Bastien" };
        var passager2 = new Passenger() { Nom = "Amano", Prenom = "Maya" };
        var passager3 = new Passenger() { Nom = "Suou", Prenom = "Tatsuya" };

        ctx.Passagers.Add(passager1);
        ctx.Passagers.Add(passager2);
        ctx.Passagers.Add(passager3);

        var vol1 = new Vol() { Depart = "Genève", Destination = "Tokyo", DateVol = new DateTime(2023, 8, 22, 12, 0, 0), PrixTicketBase = 59.99f, SiegesTotaux = 100, SiegesVides = 51 };
        var vol2 = new Vol() { Depart = "Tokyo", Destination = "Kyoto", DateVol = new DateTime(2024, 1, 15, 18, 30, 0), PrixTicketBase = 24.99f, SiegesTotaux = 100, SiegesVides = 20 };
        var vol3 = new Vol() { Depart = "Sumaru", Destination = "Tokyo", DateVol = new DateTime(2022, 11, 9, 9, 45, 0), PrixTicketBase = 21.59f, SiegesTotaux = 50, SiegesVides = 6 };
        var vol4 = new Vol() { Depart = "Sion", Destination = "Londres", DateVol = new DateTime(2022, 12, 9, 11, 30, 0), PrixTicketBase = 79.99f, SiegesTotaux = 30, SiegesVides = 0 };


        ctx.Vols.Add(vol1);
        ctx.Vols.Add(vol2);
        ctx.Vols.Add(vol3);
        ctx.Vols.Add(vol4);

        var ticket1 = new Reservation() { Passager = passager1, Vol = vol1, PrixTicketEffectif = vol1.PrixTicketBase };
        var ticket2 = new Reservation() { Passager = passager2, Vol = vol2, PrixTicketEffectif = vol2.PrixTicketBase };
        var ticket3 = new Reservation() { Passager = passager3, Vol = vol3, PrixTicketEffectif = vol3.PrixTicketBase };

        ctx.Reservations.Add(ticket1);
        ctx.Reservations.Add(ticket2);
        ctx.Reservations.Add(ticket3);

        ctx.SaveChanges();
    }

    Console.WriteLine("Affichage des vols libres :");

    var volsLibres = ctx.Vols.Where(i => i.SiegesVides > 0).ToList();

    foreach (var vol in volsLibres)
    {
        Console.WriteLine("Vol N°" + vol.IdVol + "\nDépart : " + vol.Depart + "\nDestination : " + vol.Destination + "\nDate du vol : " + vol.DateVol + "\nSièges libres : " + vol.SiegesVides + "\nPrix de base du ticket : " + vol.PrixTicketBase + " CHF\n");
    }

    Console.WriteLine("\nProgramme terminé.");

}