using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{
    public class Contract : BaseColumnField
    {

        public Int64 ContractId { get; set; }
        [Required (ErrorMessage="*")]
        [MaxLength(150)]

        public string ContractName { get; set; }
        public byte[] ContractIcon { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(150)]
        public string Label { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EntryDeadline { get; set; }
        public DateTime? ContrctFrom { get; set; }
        public DateTime? ContrctTo { get; set; }
        public string ContractDeliverables { get; set; }
        public string PrimaryManufacturer { get; set; }
        public Int64? ManufacturerId { get; set; }
        public string Website { get; set; }
        public Int64 ContractStatusId { get; set; }
        public bool IsReportable { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ContractStatus ContractStatus { get; set; }
        public virtual ICollection<ContractProduct> ContractProduct { get; set; }
        public virtual ICollection<Resource> Resource { get; set; }
       // public virtual ICollection<Contract> ActiveBuilder { get; set; }
        public virtual ICollection<ContractMarket> ContractMarket { get; set; }
        public virtual ICollection<ContractRebate> ContractRebate { get; set; }
        public virtual ICollection<ContractBuilder> ContractBuilder { get; set; }
    }

    //public class UploadResource
    //{
    //    public UploadResource()
    //    {

    //    }

    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Key]
    //    public Int64 UploadResourceId { get; set; }

    //    [Required]
    //    public Int64 ContractId { get; set; }

    //    [Required(ErrorMessage = "Market is required")]
    //    public int Martket { get; set; }

    //    [Required(ErrorMessage = "Select file to upload")]
    //    public byte[] File { get; set; }

    //    [Required(ErrorMessage = "Title is required")]
    //    [MaxLength(50)]
    //    public string Title { get; set; }

    //    [MaxLength(150)]
    //    public string Description { get; set; }
    //}


    //public class Status
    //{
    //    public Status()
    //    {

    //    }

    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Key]
    //    public Int16 StatusId { get; set; }

    //    [MaxLength(50)]
    //    [Required]
    //    public string StatusName { get; set; }

    //    public ICollection<Contract> Contract { get; set; }
    //}
}
