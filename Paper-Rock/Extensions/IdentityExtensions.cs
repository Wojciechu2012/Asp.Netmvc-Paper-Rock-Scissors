using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Paper_Rock.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetOrganizationPoints(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Points");

            return (claim != null) ? claim.Value.ToString() : "null";
        }
    }
}