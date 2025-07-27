# VSFly – Dynamic Flight Pricing System

> *ASP.NET Core 6 MVC & REST API project simulating airline dynamic pricing based on demand and lead time.*

---

## 📚 Project Description

VSFly is an academic project simulating dynamic pricing for an airline ticketing system. It comprises a backend **RESTful API** for flight and ticket management and a **client-facing MVC web application** consuming this API.

The pricing model adjusts fares in real time based on the **load factor** (seat occupancy rate) and the **time remaining before departure**, encouraging early booking and optimized seat allocation.

---

## 🧪 Technologies Used

| Type      | Name             | Version |
| --------- | ---------------- | ------- |
| Language  | C#               | 10      |
| Framework | ASP.NET Core MVC | 6.0     |
| ORM       | Entity Framework | Core 6  |
| Database  | SQL Server       | Express |
| Paradigm  | MVC / REST       |         |

---

## 🎯 Learning Objectives

* Understand RESTful API principles with ASP.NET Core
* Implement dynamic pricing logic based on business rules
* Use Entity Framework Core with Code First and migrations
* Develop a client MVC web app consuming external APIs
* Structure a multi-layered ASP.NET Core solution (API + client)

---

## 🔧 Features

* REST API with JSON output and OpenAPI documentation
* Dynamic pricing based on occupancy and time constraints
* Code-first EF Core data model with migrations
* API endpoints for flight listing, ticket booking, pricing, analytics
* MVC client with flight browser and ticket purchase

### 🔧 Bonus Features

* Swagger auto-generated documentation
* Seat booking simulation with persisted pricing logic

---

## 🧠 Language Paradigm Principles

* ✅ Separation of concerns (API vs MVC)
* ✅ Code First modeling
* ✅ Dependency Injection

### Error Handling

* Basic error handling using standard ASP.NET Core exception filters

---

## 🏗 Project Structure

### Architecture

* `VSFlyAPI/`

  * Controllers: `FlightsController`, `TicketsController`
  * Services: `PriceCalculator`
  * Data: `DbContext`, Migrations
* `VSFlyWebApp/`

  * Razor Views for flight browsing
  * HttpClient integration with API

Interaction Flow:

1. User accesses MVC front end
2. Front end calls API endpoints
3. API calculates dynamic price and stores it
4. Response sent in JSON, displayed on the UI

---

## 📘 Documentation & Diagrams

* Swagger UI: [http://localhost](#)[:xxxx](#)[/swagger](#) *(auto-generated)*

---

## ✅ Tests & Validation

* Manual testing via Swagger and MVC frontend

---

---

## 📌 Success Criteria Table

| Criterion                               | Status     | Notes                                                 |
| --------------------------------------- | ---------- | ----------------------------------------------------- |
| Core Functionality (API + MVC)          | ✅ Done     | All endpoints (list, buy, prices, stats) implemented  |
| Pricing Logic (Load Factor + Lead Time) | ✅ Done     | Logic implemented as specified (150%, 80%, 70%, base) |
| Data Persistence (EF Core Code First)   | ✅ Done     | Sale prices are saved in DB                           |
| Web API Documentation                   | ✅ Done     | Swagger enabled and accessible                        |
| MVC Client Integration                  | ✅ Done     | Frontend consumes API correctly                       |
| Code Quality (EF, LINQ, separation)     | ✅ Done     | LINQ and architecture reviewed manually               |
| Bug Count                               | ✅ Done   | No bugs observed                                |
| Demo Preparedness                       | ✅ Done     | Live MVC demo prepared for flight listing & booking   |

---

## 👤 Authors

* **Elias Borrajo** 
* **Bastien Salamin**

---

\*\*\*Project realized for the course \*\*\*\*Module 634.2)\* \*\*\*Programmation par composants - \*\*\*Entity Framework & ASP.NET Core WebAPI \*
***Instructor: Antoine Widmer, HES-SO Valais-Wallis***
\*Context: \* Bachelor of Science in Business IT, 5th Semester\*

---

## 🧾 Historical README (Original Content)

> Ce bloc est conservé pour des raisons de conformité pédagogique

### VSFly by Borrajo Elias & Salamin Bastien

Date du rendu, le 10.01.2023

* Utiliser Entity Framework afin de créer une application de gestion de vols d'un aéroport.
* L'application comprend :

  * API : Serveur à disposition de sites web souhaitant utiliser nos fonctions
  * Base de données créée via la structure du code (code first)
  * Côté client, web app d'une compagnie aérienne utilisant notre API

#### Résumé des fonctionnalités

* Le cahier des charges a été suivi, tout fonctionne du côté API & Entity Framework.
* Bonus : création d’un client MVC consommant l’API.


---

<details>
    <summary>
        <h2>
            Original Readme Archive (FR/EN)
        </h2>
    </summary>



## 2. README original (FR/EN) – non modifié
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

    
</details>

