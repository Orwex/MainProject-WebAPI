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
    [BasicAuthenticationCustomer]
    public class CustomerFacadeController : ApiController
    {
        //ILoginToken loginUser;
        LoginToken<Customer> CustomerUserLoginToken;
        public LoggedInCustomerFacade getLoginToken()
        {
            //create Customer User Facade:            
            Request.Properties.TryGetValue("customer-login-token", out object loginUser);
            CustomerUserLoginToken = (LoginToken<Customer>)loginUser;
            IFacade AirlineCompanyIFacade = FlyingCenterSystem.GetFlyingCenterSystemInstance().GetFacade(CustomerUserLoginToken);

            return (LoggedInCustomerFacade)AirlineCompanyIFacade;
        }

        //=========================================Get==================================================
        [ResponseType(typeof(IList<Flight>))]
        [HttpGet]
        [Route("api/CustomerFacadeController/GetAllFlights")]
        public IHttpActionResult GetAllFlights()
        {
            IList<Flight> flights = new List<Flight>();
            try
            {
                flights = getLoginToken().GetAllFlights();
                if (flights.Count == 0)
                    return NotFound();
                return Ok(flights);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        //=========================================Post=================================================
        [HttpPost]
        [Route("api/CustomerFacadeController/PurchaseTicket")]
        public IHttpActionResult PurchaseTicket([FromBody]Flight flight)
        {
            try
            {
                getLoginToken().PurchaseTicket(CustomerUserLoginToken, flight);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }

        }
        //=========================================put==================================================

        //=========================================Delete===============================================
        [HttpDelete]
        [Route("api/CustomerFacadeController/CancelTicket")]
        public IHttpActionResult CancelTicket([FromBody]Ticket ticket)
        {
            try
            {
                getLoginToken().CancelTicket(CustomerUserLoginToken, ticket);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }

        }
    }
}
