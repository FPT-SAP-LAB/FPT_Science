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
    
    public partial class WorkingProcess
    {
        public int id { get; set; }
        public Nullable<int> pepple_id { get; set; }
        public string work_unit { get; set; }
        public string title { get; set; }
        public Nullable<int> start_year { get; set; }
        public Nullable<int> end_year { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
