using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;

namespace CBUSA.Areas.CbusaBuilder.Models
{
    public class ProjectViewModel
    {
        public int rowcount { get; set; }
        public int RowStatusId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 ProjectCount { get; set; }
        public string ProjectName { get; set; }
        public string estimatedactual { get; set; }
        public byte[] ContractIcon { get; set; }
        public string percentage { get; set; }
        public string Quater { get; set; }
        public string year { get; set; }
        public string SubmitDate { get; set; }
        public string LotNo { get; set; }
        public Int64 StatusSelectId { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Status3 { get; set; }
        public Int64 Status1Id { get; set; }
        public Int64 Status2Id { get; set; }
        public Int64 Status3Id { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public Int32 StateId { get; set; }
        public Int32 CityId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int rowNum { get; set; }
        public string Icon { get; set; }
        public List<string> IconList { get; set; }
    }
}