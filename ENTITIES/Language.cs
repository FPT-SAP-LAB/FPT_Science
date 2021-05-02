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
    
    public partial class Language
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Language()
        {
            this.AcademicCollaborationTypeLanguages = new HashSet<AcademicCollaborationTypeLanguage>();
            this.CollaborationTypeDirectionLanguages = new HashSet<CollaborationTypeDirectionLanguage>();
            this.ArticleVersions = new HashSet<ArticleVersion>();
            this.AcademicDegreeLanguages = new HashSet<AcademicDegreeLanguage>();
            this.AcademicDegreeTypeLanguages = new HashSet<AcademicDegreeTypeLanguage>();
            this.CitationStatusLanguages = new HashSet<CitationStatusLanguage>();
            this.ConferenceConditionLanguages = new HashSet<ConferenceConditionLanguage>();
            this.ConferenceStatusLanguages = new HashSet<ConferenceStatusLanguage>();
            this.FormalityLanguages = new HashSet<FormalityLanguage>();
            this.AcademicActivityLanguages = new HashSet<AcademicActivityLanguage>();
            this.AcademicActivityTypeLanguages = new HashSet<AcademicActivityTypeLanguage>();
            this.NotificationTypeLanguages = new HashSet<NotificationTypeLanguage>();
            this.PaperStatusLanguages = new HashSet<PaperStatusLanguage>();
            this.PolicyTypeLanguages = new HashSet<PolicyTypeLanguage>();
            this.PositionLanguages = new HashSet<PositionLanguage>();
            this.ResearchAreaLanguages = new HashSet<ResearchAreaLanguage>();
            this.SpecializationLanguages = new HashSet<SpecializationLanguage>();
            this.TitleLanguages = new HashSet<TitleLanguage>();
            this.AcademicActivityPhaseLanguages = new HashSet<AcademicActivityPhaseLanguage>();
            this.PaperRewardTypeLanguages = new HashSet<PaperRewardTypeLanguage>();
            this.PaperTypeByAreaLanguages = new HashSet<PaperTypeByAreaLanguage>();
        }
    
        public int language_id { get; set; }
        public string language_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicCollaborationTypeLanguage> AcademicCollaborationTypeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollaborationTypeDirectionLanguage> CollaborationTypeDirectionLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticleVersion> ArticleVersions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicDegreeLanguage> AcademicDegreeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicDegreeTypeLanguage> AcademicDegreeTypeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CitationStatusLanguage> CitationStatusLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConferenceConditionLanguage> ConferenceConditionLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConferenceStatusLanguage> ConferenceStatusLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormalityLanguage> FormalityLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicActivityLanguage> AcademicActivityLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicActivityTypeLanguage> AcademicActivityTypeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationTypeLanguage> NotificationTypeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaperStatusLanguage> PaperStatusLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PolicyTypeLanguage> PolicyTypeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PositionLanguage> PositionLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResearchAreaLanguage> ResearchAreaLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecializationLanguage> SpecializationLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TitleLanguage> TitleLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicActivityPhaseLanguage> AcademicActivityPhaseLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaperRewardTypeLanguage> PaperRewardTypeLanguages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaperTypeByAreaLanguage> PaperTypeByAreaLanguages { get; set; }
    }
}
