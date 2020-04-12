using FlightsSystem;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAppForFlights_ServerSide.Controllers
{
    internal class BasicAuthenticationAdministratorAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
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

            if (username == "admin" && password == "9999")
            {
                //create loginToken for Admin
                ILoginToken LoginUser = FlyingCenterSystem.GetFlyingCenterSystemInstance().Login(username, password);

                actionContext.Request.Properties["login-token"] = LoginUser;
            }
            else
            {
                //stops the request - will not arrive to web api controller
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "You are not authorized");
            }
        }
    }
}