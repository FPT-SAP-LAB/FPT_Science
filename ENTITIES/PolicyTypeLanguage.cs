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
    
    public partial class PolicyTypeLanguage
    {
        public int policy_type_language_id { get; set; }
        public int policy_type_id { get; set; }
        public string policy_type_name { get; set; }
        public int language_id { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual PolicyType PolicyType { get; set; }
    }
}