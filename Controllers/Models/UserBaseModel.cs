using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Controllers.Models
{
    public class UserBaseModel
    {
        public string Name{get;set;}
        public string Email{get;set;}
        public string SelectedSurvey{get;set;}
        public int ClientId { get; set; }
        public string AppKey { get; set; }
    }
}