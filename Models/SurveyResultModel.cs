using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class SurveyResultModel
    {
        public int Surveyid { get; set; }
        public int UserId { get; set; }
        public int EntityId {get;set;}
        public int AttrId {get;set;}
        public string Value { get; set; }
        public string Type { get; set; }
    }
}