using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using may222017.Models;

namespace may222017
{
    [WebService]
    public class myServiceClass
    {
        myDbContext db;
        [WebMethod]
        public Places[] GetPlacesXML()
        {
            Places[] places = db.Places.ToArray();        
            return places;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetPlacesJSON()
        {
            Places[] places = db.Places.ToArray();
        return new JavaScriptSerializer().Serialize(places);
        }
    }
    
}