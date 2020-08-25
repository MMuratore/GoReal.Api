using GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tools.Security.Token;

namespace GoReal.Api.Infrastrucutre
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthRequiredAttribute : TypeFilterAttribute
    {
        public AuthRequiredAttribute(Role roles = Role.None) : base(typeof(AuthRequiredFilter)) 
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthRequiredFilter : IAuthorizationFilter
    {
        private Role RequiredRoles { get; set; }
        private Role UserRoles { get; set; }

        public AuthRequiredFilter(Role roles = Role.None)
        {
            RequiredRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ITokenService _tokenService = (ITokenService)context.HttpContext.RequestServices.GetService(typeof(ITokenService));

            context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizations);

            string token = authorizations.SingleOrDefault(t => t.StartsWith("Bearer "));

            if (token is null)
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            else
            {
                IEnumerable<string> properties = new List<string>() { "UserId", "LastName", "FirstName", "Email", "Roles" };
                IDictionary<string, string> user = _tokenService.DecodeToken(token, properties);

                if (user is null) 
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                else
                {
                    UserRoles = (Role)int.Parse(user["Roles"]);

                    if (!UserRoles.HasFlag(RequiredRoles))
                        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                    else
                        context.RouteData.Values.Add("userId", user["UserId"]);
                }
            }

        }
    }
}
