using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Models
{
    public class RequiredQuestionModel
    {
        public int QuestionId { get; set; }
        public string Value { get; set; }
        public int? QuestionOrder { get; set; }
        public bool? IsDependency { get; set; }
        public int? DependentAttrId {get;set;}
        public int? DependentEntityId { get; set; }
    }
}