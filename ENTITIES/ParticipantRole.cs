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
    
    public partial class ParticipantRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParticipantRole()
        {
            this.Participants = new HashSet<Participant>();
            this.PlanParticipants = new HashSet<PlanParticipant>();
        }
    
        public int participant_role_id { get; set; }
        public string participant_role_name { get; set; }
        public bool need_payed { get; set; }
        public Nullable<double> price { get; set; }
        public int phase_id { get; set; }
    
        public virtual AcademicActivityPhase AcademicActivityPhase { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Participant> Participants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanParticipant> PlanParticipants { get; set; }
    }
}
