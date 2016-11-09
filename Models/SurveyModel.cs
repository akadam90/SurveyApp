using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class SurveyModel
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedOn { get; set; }
        public IList<EntityModel> questions { get; set; }
        public IList<EntityModel> RequiredQuestions { get; set; }
    }
}