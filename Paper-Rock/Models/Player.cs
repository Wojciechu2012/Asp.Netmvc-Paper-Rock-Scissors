using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Paper_Rock.Models
{
    public class Player:IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Player> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(new Claim("Points", this.Points.ToString()));
            // Add custom user claims here
            return userIdentity;
        }

        public int Points { get; set; }

        public virtual ICollection<Room> Room { get; private set; }




        internal void AddUserPoints(int v)
        {
            Points += v;
        }
    }



    
}