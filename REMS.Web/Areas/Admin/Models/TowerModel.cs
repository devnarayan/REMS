using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REMS.Web.Areas.Admin.Models
{
    public class TowerModel
    {
        public int TowerID { get; set; }
        public string TowerName { get; set; }
        public string TowerNo { get; set; }
        public string Block { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ProjectTypeID { get; set; }
        public string PName { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string ReceiptPrefix { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public string OfficeAddress { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string Tehsil { get; set; }
        public string Jurisdiction { get; set; }
        public Nullable<System.DateTime> PossessionDate { get; set; }
        public string STReceiptNo { get; set; }
        public string AuthoritySign { get; set; }
        public Nullable<bool> Security { get; set; }
        public Nullable<bool> PlayArea { get; set; }
        public Nullable<bool> PowerBackup { get; set; }
        public Nullable<bool> SwimmingPool { get; set; }
        public Nullable<bool> Gym { get; set; }
        public Nullable<bool> Garden { get; set; }
        public Nullable<bool> Library { get; set; }
        public Nullable<bool> CommunityHall { get; set; }
        public Nullable<bool> ClubHourse { get; set; }
        public Nullable<bool> JoggingTrack { get; set; }
        public Nullable<bool> Lift { get; set; }
        public Nullable<bool> Internet { get; set; }
        public Nullable<bool> InternalRoad { get; set; }
        public string PossessionDateSt { get; set; }
    }
}