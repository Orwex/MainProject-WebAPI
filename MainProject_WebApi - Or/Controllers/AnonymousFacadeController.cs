using FlightsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAppForFlights_ServerSide.Controllers
{
    public class AnonymousFacadeController : ApiController
    {
        public AnonymousUserFacade getLoginToken()
        {
            //create Anonymous User Facade:
            ILoginToken AnonymUserLoginToken = FlyingCenterSystem.GetFlyingCenterSystemInstance().Login(null, null);
            IFacade AnonymIFacade = FlyingCenterSystem.GetFlyingCenterSystemInstance().GetFacade(AnonymUserLoginToken);

            return (AnonymousUserFacade)AnonymIFacade;
        }
        #region Get
        /// <summary>
        /// Get all flight items - GET main route
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetAllFlights")]
        public IHttpActionResult GetAllFlights()
        {
            IList<Flight> flights = new List<Flight>();
            //get all flights from FACADE:                
            flights = getLoginToken().GetAllFlights(); 

            //check if empty
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        /// <summary>
        ///  Get all airline company items - GET main route
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IList<AirlineCompany>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetAllAirlineCompanies")]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            IList<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            //get all flights from FACADE:                
            airlineCompanies = getLoginToken().GetAllAirLineCompanies(); 

            //check if empty
            if (airlineCompanies.Count == 0)
            {
                return NotFound();
            }
            return Ok(airlineCompanies);
        }

        /// <summary>
        /// Get All Flights and their Vacancy in a Dictionary
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Dictionary<Flight, int>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetAllFlightsVacancy", Name = "GetAllFlightsVacancy")]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> FlightVacancyPair = new Dictionary<Flight, int>();
            FlightVacancyPair = getLoginToken().GetAllFlightVacancy();
            //check if empty
            if (FlightVacancyPair.Count == 0)
            {
                return NotFound();
            }
            return Ok(FlightVacancyPair);
        }

        /// <summary>
        /// Get flight item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("api/AnonymousFacade/Flight/{id}", Name = "GetFlightById")]
        public IHttpActionResult GetFlightById(int id)
        {
            Flight flight = getLoginToken().GetFlightByID(id);
            //check if empty
            if (flight.FlightID == 0)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        /// <summary>
        /// Get all Flights By Origin Country
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns></returns>
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetFlightsByOriginCountry/{CountryCode}", Name = "GetFlightsByOriginCountry")]
        public IHttpActionResult GetFlightsByOriginCountry(int CountryCode)
        {
            IList<Flight> flights = new List<Flight>();
            //get all flights from FACADE:                
            if (CountryCode != 0)
                flights = getLoginToken().GetFlightsByOriginCountry(CountryCode);
            else
            {
                return BadRequest($"CountryCode {CountryCode} is wrong, please try again");
            }
            

            //check if empty
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        /// <summary>
        /// Get Flights By Destination Country
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns></returns>
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetFlightsByDestinationCountry/{CountryCode}", Name = "GetFlightsByDestinationCountry")]
        public IHttpActionResult GetFlightsByDestinationCountry(int CountryCode)
        {
            IList<Flight> flights = new List<Flight>();
            //get all flights from FACADE: 
            if (CountryCode != 0)
                flights = getLoginToken().GetFlightsByDestinationCountry(CountryCode);
            else
            {
                return BadRequest($"CountryCode {CountryCode} is wrong, please try again");                
            }

            //check if empty
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        /// <summary>
        /// Get Flights By Departure Date
        /// </summary>
        /// <param name="departureDate"></param>
        /// <returns></returns>
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetFlightsByDepartureDate/{departureDate}", Name = "GetFlightsByDepartureDate")]
        public IHttpActionResult GetFlightsByDepartureDate(DateTime departureDate)
        {
            IList<Flight> flights = new List<Flight>();
            //get all flights from FACADE:                
            flights = getLoginToken().GetFlightsByDepartureDate(departureDate);

            //check if empty
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        /// <summary>
        /// Get Flights By Landing Date
        /// </summary>
        /// <param name="landingDate"></param>
        /// <returns></returns>
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/AnonymousFacade/GetFlightsByLandingDate/{landingDate}", Name = "GetFlightsBylandingDate")]
        public IHttpActionResult GetFlightsByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = new List<Flight>();
            //get all flights from FACADE:                
            flights = getLoginToken().GetFlightsByLandingDate(landingDate);

            //check if empty
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        #endregion

        //Post - nothing
        //put - nothing
        //Delete - nothing
    }
}
