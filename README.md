# VSFly by Borrajo Elias & Salamin Bastien
Date du rendu, le 10.01.2023

* Utiliser Entity Framework afin de créer une application de gestion de vols d'un aéroport.
* L'application comprend : 
    - API : Serveur à disposition de sites web shouaitant utiliser nos fonctions
    - Database crée par Entity Framework via la structure du code (code first)
    - Côté client, web app d'une compagnie aérienne utilisant notre API

# Resumé des fonctionalitées
* Le cahier des charges a été suivi, tout fonctionne du côté API & Etity FrameWork
* Bonus : On a créé un client MVC qui peut faire des requêtes à notre API.

# Cahier des charges - EN
## Introduction
On the basis of the EF model seen in the course, you must design an aircraft price management application for the airline VSFly.

## Constraints
For each flight available in the database, the other partner websites (ebooker / skyscanner type) can buy tickets for their customers through their websites as a front-end using webAPI requests from the BLL of their sites.


For each flight a **base price** is offered by the airline. 

Rules exist to maximize the filling of the aircraft and the total gain on all seats. 

For this there are 2 variables (the filling rate of the plane and the deadline of the flight in relation to the date of purchase of the ticket). The calculation of the **sale price** must be done on the WebAPI server side and be returned to the partner site on each request. 

In the database managed by Entity Framework, the **sale price** of each ticket must be saved.

1.	If the airplane is more than 80% full regardless of the date:
    * sale price = 150% of the base price
    
2.	If the plane is filled less than 20% less than 2 months before departure:
    * sale price = 80% of the base price
    
3.	If the plane is filled less than 50% less than 1 month before departure:
    * sale price = 70% of the base price
    
4.	In all other cases:
    * sale price = base price


## Delivery
The result consists of 2 Visual Studio solutions:
1)	Partner site
    * a.	With an MVC presentation layer (.net core) for
        * i.	List of flights
        * ii.	Buy tickets on available flights (no change or cancellation possible)

2)	VSAFly's WebAPI
    * a.	With a webAPI layer
        * i.	A controller accepting RESTfull requests and returning the data in JSON format
        * ii.	Requests to be processed:
      
    * a.    Return all available flights (not full)
    * b.	Return the sale price of a flight
    * c.	Buying a ticket on a flight
    * d.	Return the total sale price of all tickets sold for a flight
    * e.	Return the average sale price of all tickets sold for a destination (multiple flights possible)
    * f.	Return the list of all tickets sold for a destination with the first and last name of the travelers and the flight number as well as the sale price of each ticket.
    * b.	With an EntityFramework core layer to access the database as illustrated in the following figure.


## Organisation
Group of 2 students or alone.

The 2 solutions must be in a zip file on cyberlearn
Evaluation
The final grade will depend on:
1.	Your involvement in the project
2.	The present functionalities
3.	Number of bugs
4.	Quality of the code (LINQ request, correct use of EF and WebAPI)
5.	Answer to the questions asked during the demo
