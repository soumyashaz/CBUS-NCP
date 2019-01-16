using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
    public class DashboardBuilderContractProjectListRepository
    {

        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public string BuilderName { get; set; }
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public Int64 ContractId { get; set; }
        public string ContractName { get; set; }
        public string VendorName { get; set; }
        public string ContractEnrolledDate { get; set; }
        public Int64 TotalProjects { get; set; }
        public Int64 ReportedProjects { get; set; }
        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string LotNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }        
        public string ProjectCreatedOn { get; set; }
        public int ProjectStatus { get; set; }
        public double ListReport { get; set; }
    }
}
