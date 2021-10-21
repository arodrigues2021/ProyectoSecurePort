#region Using

using System;
using System.Web;

#endregion

public class SessionTimeOut
{
    public static bool IsSessionTimedOut()
    {
        HttpContext ctx = HttpContext.Current;
        if (ctx.Session.SessionID == null)
        {
            return true; 
        }
        //Se comprueba si se ha generado una nueva sesión en esta petición
        if (ctx.Session.IsNewSession)
        {
            string sessionCookie = ctx.Request.Headers["Cookie"];

            if (sessionCookie != null) 
            {
                return true;
            }

            return false;
        }

        return false;
    }
    
}
