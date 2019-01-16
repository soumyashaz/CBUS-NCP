using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.CbusaBuilder.Models
{
    public class ContractBuilderViewModel
    {
        public Int64 ContractId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 ResourceCategoryId { get; set; }
        public List<string> Websiteslist { get; set; }
        public List<string> Manufactererlist { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 RowStatus { get; set; }
        public string ContractName { get; set; }
        public string Website { get; set; }
        public string ContractIcon { get; set; }
        public string DayToGo { get; set; }
        public string Complience { get; set; }
        public string Estimated { get; set; }
        public string Percentage { get; set; }
        public string ManuFacturerName { get; set; }
        public string BuilderCount { get; set; }
        public string BuilderJoin { get; set; }
        public string ContractStatus { get; set; }
        public string NationalContractPartner { get; set; }
        public string ContractFrom { get; set; }
        public string EntryDeadline { get; set; }
        public string EntryDeadlineStatus { get; set; }
        public string ContractTo { get; set; }
        public string ContractDeliverables { get; set; }
        public  List<string> ContractDeliverableslist { get; set; }
        public string ProductList { get; set; }
        public string ActionStatus { get; set; }
        public string ProductCategoryName { get; set; }
        public List<Product> Products { get; set; }
        public int rowcount { get; set; }
    }
}