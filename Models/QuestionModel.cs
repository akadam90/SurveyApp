using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyApp.Models
{
    public class QuestionModel
    {
        public EntityModel entity { get; set; }
        public IList<AttributeModel> LocalAttributes { get; set; }
        public IList<EntityModel> LocalEntities { get; set; }
    }
}
