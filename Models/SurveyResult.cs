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
    
    public partial class SurveyResult
    {
        public int SurveyId { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int AttrId { get; set; }
        public string Value { get; set; }
    
        public virtual Attribute Attribute { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual User User { get; set; }
    }
}