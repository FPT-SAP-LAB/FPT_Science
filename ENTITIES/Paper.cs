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
    
    public partial class Paper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paper()
        {
            this.ConferenceSupports = new HashSet<ConferenceSupport>();
            this.AuthorPapers = new HashSet<AuthorPaper>();
            this.PaperWithCriterias = new HashSet<PaperWithCriteria>();
            this.RequestPapers = new HashSet<RequestPaper>();
            this.Comments = new HashSet<Comment>();
        }
    
        public int paper_id { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> publish_date { get; set; }
        public string link_doi { get; set; }
        public string link_scholar { get; set; }
        public string journal_name { get; set; }
        public string page { get; set; }
        public string vol { get; set; }
        public string company { get; set; }
        public string index { get; set; }
        public int paper_type_id { get; set; }
        public Nullable<int> file_id { get; set; }
    
        public virtual File File { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConferenceSupport> ConferenceSupports { get; set; }
        public virtual PaperType PaperType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthorPaper> AuthorPapers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaperWithCriteria> PaperWithCriterias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestPaper> RequestPapers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
