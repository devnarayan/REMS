//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventMaster
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string EventDesc { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public Nullable<int> EventType { get; set; }
    }
}
