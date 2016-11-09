using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyApp.Models
{
    public class EntityModel
    {
        public int EntityId { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Id { get; set; }
        public int? Type { get; set; }
        public string Value { get; set; }
        public int? ParentQuestionId { get; set; }
        public bool? IsDependency { get; set; }
        public int? DependentAttrId { get; set; }
        public int? DependentEntityId { get; set; }
        public int Indent { get; set; }
        public bool? IsRequired { get; set; }
        public int? QuestionOrder { get; set; }

        
        public IList<AttributeModel> DependentAttributes { get; set; } //Attributes which are displayed dependent on / on selection of this entity (dropdown) can be a textbox or radio or other dropdown(entity)
        public IList<EntityModel> DependentEntities { get; set; } ////Entities i.e Questions/Dropdowns which are displayed dependent on selection of this entity(dropdown) eg like another subquestion /another dropdown
        
        //Incase of question these are immediate attributes n incase of dropdowns these are options
        public IList<AttributeModel> ChildAttributes { get; set; }

        //Incase of question these are immediate subquestions n incase of dropdowns nothing for now
        public IList<EntityModel> ChildEntities { get;set;} //subquestion (not dependent but displayed with it )

    }
}
