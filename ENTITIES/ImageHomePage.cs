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
    
    public partial class ImageHomePage
    {
        public bool is_wallpaper { get; set; }
        public int account_id { get; set; }
        public bool is_active { get; set; }
        public int file_id { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual File File { get; set; }
    }
}
