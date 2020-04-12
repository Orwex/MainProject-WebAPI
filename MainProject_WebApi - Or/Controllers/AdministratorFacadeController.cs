using FlightsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAppForFlights_ServerSide.Controllers
{
    [BasicAuthenticationAdministrator]
    public class AdministratorFacadeController : ApiController
    {
        //ILoginToken loginUser;
        LoginToken<Administrator> AdminLoginToken;
        public LoggedInAdministratorFacade getAdminLoginToken()
        {
            //create Anonymous User Facade:
            Request.Properties.TryGetValue("login-token", out object loginUser);
            AdminLoginToken = (LoginToken<Administrator>)loginUser;
            IFacade LoginIFacade = FlyingCenterSystem.GetFlyingCenterSystemInstance().GetFacade(AdminLoginToken);

            return (LoggedInAdministratorFacade)LoginIFacade;
        }

        //============Get=============
        //there is no functions here
        
        //============================Post=============================
        
        /// <summary>
        /// Add / create new airline Company
        /// </summary>
        /// <param name="airlineCompany"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/AdministratorFacade/CreateNewAirline")]
        public IHttpActionResult PostNewAirline([FromBody]AirlineCompany airlineCompany)
        {
            try
            {
                getAdminLoginToken().CreateNewAirline(AdminLoginToken, airlineCompany);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }          
            
        }
        /// <summary>
        /// Add / create new airline Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/AdministratorFacade/CreateNewCustomer")]
        public IHttpActionResult CreateNewCustomer([FromBody]Customer customer)
        {
            try
            {
                getAdminLoginToken().CreateNewCustomer(AdminLoginToken, customer);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }

        //=============================put==============================

        /// <summary>
        /// Update the airline details in the id specified.
        /// </summary>
        /// <param name="airline"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/AdministratorFacade/UpdateAirlineDetails/{id}")]
        public IHttpActionResult UpdateAirlineDetails([FromBody]AirlineCompany airline, [FromUri]int id)
        {
            try
            {
                airline.Airline_ID = id;
                getAdminLoginToken().UpdateAirlineDetails(AdminLoginToken, airline);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }

        /// <summary>
        /// Update the Customer details in the id specified.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/AdministratorFacade/UpdateCustomerDetails/{id}")]
        public IHttpActionResult UpdateCustomerDetails([FromBody]Customer customer, [FromUri]int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest($"Customer id {id} is wrong, please try again");
                else
                    customer.Customer_ID = id;
                getAdminLoginToken().UpdateCustomerDetails(AdminLoginToken, customer);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }

        //==========================Delete===============================

        /// <summary>
        /// Remove the Airline specified.
        /// </summary>
        /// <param name="airline"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/AdministratorFacade/RemoveAirline")]
        public IHttpActionResult RemoveAirline(AirlineCompany airline)
        {
            try
            {
                getAdminLoginToken().RemoveAirline(AdminLoginToken, airline);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }

        /// <summary>
        /// Remove the Customer specified.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/AdministratorFacade/RemoveCustomer")]
        public IHttpActionResult RemoveCustomer(Customer customer)
        {
            try
            {
                getAdminLoginToken().RemoveCustomer(AdminLoginToken, customer);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
    }
}
