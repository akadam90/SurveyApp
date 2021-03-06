//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SurveyApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attribute
    {
        public Attribute()
        {
            this.Attributes1 = new HashSet<Attribute>();
            this.SurveyResults = new HashSet<SurveyResult>();
        }
    
        public int AttrId { get; set; }
        public Nullable<int> ParentEntityId { get; set; }
        public Nullable<int> AttrDependencyId { get; set; }
        public string Value { get; set; }
        public int Type { get; set; }
        public Nullable<int> EntityDepId { get; set; }
        public int Indent { get; set; }
        public Nullable<int> MaxCharacters { get; set; }
    
        public virtual ICollection<Attribute> Attributes1 { get; set; }
        public virtual Attribute Attribute1 { get; set; }
        public virtual AttributeType AttributeType { get; set; }
        public virtual ICollection<SurveyResult> SurveyResults { get; set; }
    }
}
