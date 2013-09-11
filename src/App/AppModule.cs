﻿namespace App
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Authentication.Forms.Owin;
    using Nancy.ModelBinding;
    using Owin;

    public class AppModule : NancyModule
    {
        public AppModule(IUserManager userManager)
        {
            Get["/getclaimscount"] = _ =>
                                     {
                                         var claimsPrincipal = this.GetClaimsPrincipal();
                                         if (claimsPrincipal == null)
                                         {
                                             return HttpStatusCode.Unauthorized;
                                         }
                                         return claimsPrincipal.Claims.Count().ToString(CultureInfo.InvariantCulture);
                                     };

            Post["/login"] = paramaters =>
                             {
                                 var login = this.Bind<Login>();
                                 Guid userId;
                                 if (!userManager.Authenticate(login.Username, login.Password, out userId))
                                 {
                                     return HttpStatusCode.Unauthorized;
                                 }
                                 return this.LoginAndRedirect(userId);
                             };
        }

        private class Login
        {
            public string Username { get; set; }
 
            public string Password { get; set; }
        }
    }
}