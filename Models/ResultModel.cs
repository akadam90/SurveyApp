using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class ResultModel
    {
        public int SurveyId { get; set; }
        public int UserId { get; set; }
        public IList<SurveyResultModel> results { get; set; }
    }
}