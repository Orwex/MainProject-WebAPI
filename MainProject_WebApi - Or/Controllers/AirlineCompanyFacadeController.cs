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
    [BasicAuthenticationCompany]
    public class AirlineCompanyFacadeController : ApiController
    {
        //ILoginToken loginUser;
        LoginToken<AirlineCompany> AirlineUserLoginToken;
        public LoggedInAirlineFacade getLoginToken()
        {
            //create AirlineCompany User Facade:            
            Request.Properties.TryGetValue("airline-company-login-token", out object loginUser);
            AirlineUserLoginToken = (LoginToken<AirlineCompany>)loginUser;
            IFacade AirlineCompanyIFacade = FlyingCenterSystem.GetFlyingCenterSystemInstance().GetFacade(AirlineUserLoginToken);

            return (LoggedInAirlineFacade)AirlineCompanyIFacade;
        }
        
        //Get
        [ResponseType(typeof(List<Ticket>))]
        [HttpGet]
        [Route("api/CompanyFacadeController/GetAllTickets")]
        public IHttpActionResult GetAllTickets()
        {
            IList<Ticket> tickets = new List<Ticket>();
            try
            {
                tickets = getLoginToken().GetAllTickets(AirlineUserLoginToken);
                if (tickets.Count == 0)
                    return NotFound();
                return Ok(tickets);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/CompanyFacadeController/GetAllFlights")]
        public IHttpActionResult GetAllFlights()
        {
            IList<Flight> flights = new List<Flight>();
            try
            {
                flights = getLoginToken().GetAllFlights(AirlineUserLoginToken);
                if (flights.Count == 0)
                    return NotFound();
                return Ok(flights);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        
        //Post
        [HttpPost]
        [Route("api/CompanyFacadeController/CreateFlight")]
        public IHttpActionResult CreateFlight([FromBody]Flight flight)
        {
            try
            {
                getLoginToken().CreateFlight(AirlineUserLoginToken, flight);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        
        //put
        [HttpPut]
        [Route("api/CompanyFacadeController/UpdateFlight/{id}")]
        public IHttpActionResult UpdateFlight(Flight flight, [FromUri] int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest($"id {id} is wrong, please try again");
                else
                {
                    flight.FlightID = id;
                    getLoginToken().UpdateFlight(AirlineUserLoginToken, flight);
                    return Ok();
                }
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        /// <summary>
        /// Change Airline Company Password
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/CompanyFacadeController/ChangeMyPassword/{oldPassword}/{newPassword}/{id}")]
        public IHttpActionResult ChangeMyPassword([FromUri]string oldPassword, 
            [FromUri]string newPassword, [FromUri]int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest($"id {id} is wrong, please try again");
                else
                {
                    if (oldPassword == newPassword)
                        return BadRequest($"Your old Password {oldPassword} is the same as the " +
                            $"new password {newPassword}. Please try again.");
                    getLoginToken().ChangeMyPassword(AirlineUserLoginToken, oldPassword, newPassword);
                    return Ok();
                }
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        [HttpPut]
        [Route("api/CompanyFacadeController/ModifyAirlineDetails/{id}")]
        public IHttpActionResult ModifyAirlineDetails([FromBody]AirlineCompany airline, [FromUri] int id )
        {
            try
            {
                if (id == 0)
                    return BadRequest($"id {id} is wrong, please try again");
                else
                {
                    getLoginToken().ModifyAirlineDetails(AirlineUserLoginToken, airline);
                    return Ok();
                }
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        
        //Delete
        [HttpDelete]
        [Route("api/CompanyFacadeController/CancelFlight/{id}")]
        public IHttpActionResult CancelFlight([FromUri]int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest($"id {id} is wrong, please try again");
                else
                {
                    Flight flight = getLoginToken().GetFlightByID(id);
                    getLoginToken().CancelFlight(AirlineUserLoginToken, flight);
                    return Ok();
                }
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
    }
}
