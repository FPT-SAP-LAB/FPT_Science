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
    
    public partial class ConferenceParticipant
    {
        public int people_id { get; set; }
        public int conference_support_id { get; set; }
        public int title_id { get; set; }
        public int office_id { get; set; }
    
        public virtual Office Office { get; set; }
        public virtual Person Person { get; set; }
        public virtual ConferenceSupport ConferenceSupport { get; set; }
        public virtual Title Title { get; set; }
    }
}
