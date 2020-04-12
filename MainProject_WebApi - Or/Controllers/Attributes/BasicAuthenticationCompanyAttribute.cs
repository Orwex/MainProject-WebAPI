using FlightsSystem;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAppForFlights_ServerSide.Controllers
{
    internal class BasicAuthenticationCompanyAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool loggedIn = false;
            //got username + password here in server
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                    "You must send user name and password in basic authentication");
                return;
            }
            string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
            string decodedAuthenticationToken = Encoding.UTF8.GetString(
                Convert.FromBase64String(authenticationToken));

            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
            string username = usernamePasswordArray[0];
            string password = usernamePasswordArray[1];

            //search the username and password in the DB (with admin user Facade):
            ILoginToken LoginUser = FlyingCenterSystem.GetFlyingCenterSystemInstance().Login("admin", "9999");
            LoginToken<Administrator> AdminLoginToken = (LoginToken<Administrator>)LoginUser;
            LoggedInAdministratorFacade AdminLoginIFacade = (LoggedInAdministratorFacade)FlyingCenterSystem.GetFlyingCenterSystemInstance().GetFacade(AdminLoginToken);
            IList<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            airlineCompanies = AdminLoginIFacade.GetAllAirLineCompanies();

            //Add the request to the table in DB:
            AdminLoginIFacade.AddRequestToTableInDB(AdminLoginToken, username);

            if (!AdminLoginIFacade.IsUserBlocked(AdminLoginToken, username))
            {
                foreach (AirlineCompany alc in airlineCompanies)
                {
                    if (username == alc.UserName && password == alc.Password)
                    {
                        loggedIn = true;

                        //create loginToken for AirlineCompany
                        ILoginToken AirlineUserLoginToken = FlyingCenterSystem.GetFlyingCenterSystemInstance().Login(username, password);
                        actionContext.Request.Properties["login-airline-company"] = alc;
                        actionContext.Request.Properties["airline-company-login-token"] = AirlineUserLoginToken;
                    }
                    if (username == alc.UserName && password != alc.Password)
                    {
                        loggedIn = true;

                        //Add the request to the table in DB:
                        AdminLoginIFacade.AddRequestToTableInDB(AdminLoginToken, username);
                        //if times of login from the same user more than 3 - block the user:
                        AdminLoginIFacade.CheckIfBlockUser(AdminLoginToken, username);
                   
                        string answerWrongpassword = "Your password was wrong.";
                        if (AdminLoginIFacade.IsUserBlocked(AdminLoginToken, username))
                        {
                            answerWrongpassword += " Your user was blocked.";
                        }

                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                        answerWrongpassword);
                    }
                }
                if (!loggedIn)
                {
                    //stops the request - will not arrive to web api controller
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                        "You are not authorized. Your Username is not registered.");
                }
            }
            else
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                        "You are not authorized. Your user was blocked.");
        }
    }
}