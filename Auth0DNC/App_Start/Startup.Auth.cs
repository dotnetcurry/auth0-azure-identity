﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Auth0DNC.Models;
using System.Web;
using System.Web.Helpers;

namespace Auth0DNC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var provider = new Auth0.Owin.Auth0AuthenticationProvider
            {
                OnReturnEndpoint = (context) =>
                {
                    // xsrf validation
                    if (context.Request.Query["state"] != null && context.Request.Query["state"].Contains("xsrf="))
                    {
                        var state = HttpUtility.ParseQueryString(context.Request.Query["state"]);
                        AntiForgery.Validate(context.Request.Cookies["__RequestVerificationToken"], state["xsrf"]);
                    }

                    return System.Threading.Tasks.Task.FromResult(0);
                }
            };

            app.UseAuth0Authentication(
                clientId: System.Configuration.ConfigurationManager.AppSettings["auth0:ClientId"],
                clientSecret: System.Configuration.ConfigurationManager.AppSettings["auth0:ClientSecret"],
                domain: System.Configuration.ConfigurationManager.AppSettings["auth0:Domain"],
                provider: provider);
        }
    }
}