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
    
    public partial class Question
    {
        public int question_id { get; set; }
        public int form_id { get; set; }
        public string title { get; set; }
        public int answer_type_id { get; set; }
        public bool is_compulsory { get; set; }
    
        public virtual AnswerType AnswerType { get; set; }
        public virtual Form Form { get; set; }
        public virtual QuestionOption QuestionOption { get; set; }
    }
}
