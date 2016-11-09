using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Repositories.Interfaces.Models
{
    public class SurveyTakerInfoModel
    {
        //public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Link { get; set; }

        public string Survey { get; set; }
        
        //public int Application { get; set; }
        //public int ClientId { get; set; }
       

    }
}