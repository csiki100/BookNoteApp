using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    ///<summary>
    ///Collection of extension methods
    ///</summary>
    public static class  Extensions
    {
        ///<summary>
        ///Adds Application-Error header to error responses
        ///</summary>
        public static void AddApplicationError(this HttpResponse response, string message )
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
        }
    }
}