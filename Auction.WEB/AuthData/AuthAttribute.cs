using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.WEB.AuthData
{
    public class AuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["UserId"] != null)
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("~/Auth/Login");
                return false;
            }
            
        }
    }
}