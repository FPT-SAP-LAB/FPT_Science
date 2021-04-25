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
    
    public partial class ArticleVersion
    {
        public int article_version_id { get; set; }
        public System.DateTime publish_time { get; set; }
        public string version_title { get; set; }
        public string article_content { get; set; }
        public int article_id { get; set; }
        public int language_id { get; set; }
        public Nullable<int> account_id { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Article Article { get; set; }
        public virtual Language Language { get; set; }
    }
}
