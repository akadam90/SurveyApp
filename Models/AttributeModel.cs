using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyApp.Models
{
    public class AttributeModel
    {
        public int AttrId { get; set; }
        public int? ParentEntityId { get; set; }
        public int? ParentAttrId { get; set; }
        public int? AttrDependencyId { get; set; }
        public string Value {get;set;}
        public int? Type {get;set;}
        public int Indent { get; set; }
        public int? MaxCharacters { get; set; }

        public IList<AttributeModel> DependentAttributes { get; set; } //Attributes which are displayed dependent on / on selection of this attr
        public IList<EntityModel> DependentEntities { get; set; } ////Entities i.e Questions which are displayed dependent on selection of this attr eg like another subquestion after a chechbox or radio is selected
        public IList<AttributeModel> ChildAttributes { get; set; } // incase of dropdown
        public IList<EntityModel> ChildEntities { get; set; }    // I doubt this can b one of the scenario
    }
}
