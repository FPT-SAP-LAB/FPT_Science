//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENTITIES
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActivityInfo
    {
        public int article_id { get; set; }
        public Nullable<int> activity_id { get; set; }
    
        public virtual Article Article { get; set; }
        public virtual AcademicActivity AcademicActivity { get; set; }
    }
}
