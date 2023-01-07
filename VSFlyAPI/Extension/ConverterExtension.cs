using Microsoft.EntityFrameworkCore;
using VSFly;
using VSFly.Models;

namespace VSFlyAPI.Extension
{
    public static class ConverterExtension
    {
        // ENTITY MODELS ------> To API MODELS

        /// <summary>
        /// Converts a Booking entity model to a Booking API model.
        /// </summary>
        /// <param name="bookingEF">The Booking entity model to be converted.</param>
        /// <returns>A Booking API model representing the input Booking entity model.</returns>
        public static Models.BookingM ConvertToBookingMAPI(this VSFly.Models.Booking bookingEF)
        {
            Models.BookingM bookingMAPI = new Models.BookingM();

            bookingMAPI.Id = bookingEF.Id;
            bookingMAPI.SalePrice = bookingEF.SalePrice;
            bookingMAPI.IdPassenger = bookingEF.Passenger.Id;
            bookingMAPI.IdFlight = bookingEF.Flight.FlightNo;

            return bookingMAPI;
        }

        /// <summary>
        /// Converts a Flight entity model to a Flight API model.
        /// </summary>
        /// <param name="flightEF">The Flight entity model to be converted.</param>
        /// <returns>A Flight API model representing the input Flight entity model.</returns>
        public static Models.FlightM ConvertToFlightMAPI(this VSFly.Models.Flight flightEF)
        {
            Models.FlightM flightMAPI = new Models.FlightM();

            flightMAPI.FlightNo = flightEF.FlightNo;
            flightMAPI.Departure = flightEF.Departure;
            flightMAPI.Destination = flightEF.Destination;
            flightMAPI.Date = flightEF.Date;
            flightMAPI.TotalSeats = flightEF.TotalSeats;
            flightMAPI.FreeSeats = flightEF.FreeSeats;
            flightMAPI.BasePrice = flightEF.BasePrice;

            return flightMAPI;
        }

        /// <summary>
        /// Converts a Booking entity model to a ResumeBookedTicket API model.
        /// </summary>
        /// <param name="bookingEF">The Booking entity model to be converted.</param>
        /// <returns>A ResumeBookedTicket API model representing the input Booking entity model.</returns>
        public static Models.ResumeBookedTicket ConvertToResumeTicketMAPI( this VSFly.Models.Booking bookingEF)
        {
            Models.ResumeBookedTicket resumeTicket = new Models.ResumeBookedTicket();
            resumeTicket.PassengerName = bookingEF.Passenger == null ? "[deleted User]" : bookingEF.Passenger.Name;
            resumeTicket.FlightNumber = bookingEF.Flight.FlightNo;
            resumeTicket.TicketSalePrice = bookingEF.SalePrice;

            return resumeTicket;
        }

        // API MODELS ------> To ENTITY MODELS
        // ATTENTION ! Faire un AWAIT QUAND ON APPELLE LA FONCTION ICI !

        /// <summary>
        /// Converts a Booking API model to a Booking entity model.
        /// </summary>
        /// <param name="bookingMAPI">The Booking API model to be converted.</param>
        /// <param name="context">The database context used to retrieve passenger and flight information.</param>
        /// <returns>A Booking entity model representing the input Booking API model.</returns>
        public static async Task<VSFly.Models.Booking> ConvertToBookingEF(this Models.BookingM bookingMAPI, VSFlyContext context)
        {
            VSFly.Models.Booking modelBookingEF = new VSFly.Models.Booking();

            modelBookingEF.Id = bookingMAPI.Id;
            modelBookingEF.SalePrice = bookingMAPI.SalePrice;
            modelBookingEF.Passenger = await context.Passengers.FindAsync(bookingMAPI.IdPassenger);
            modelBookingEF.Flight        = await context.Flights.FindAsync(bookingMAPI.IdFlight);
           
            return modelBookingEF;
        }
    }
}
