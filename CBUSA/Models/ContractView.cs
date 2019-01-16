using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using FluentValidation;

namespace CBUSA.Models
{
    public class ContractView //: IValidatableObject
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int64 ContractId { get; set; }

        [DisplayName("Contract Name")]
        [Required(ErrorMessage = "Contract name is required")]
        [MaxLength(150)]
        public string ContractName { get; set; }

        [DisplayName("Label")]
        [Required(ErrorMessage = "Label is required")]
        [MaxLength(150)]
        public string Label { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "Please select status")]
        public Int16 StatusId { get; set; }

        [DisplayName("Estimated Start Date")]
        [Required(ErrorMessage = "Estimated start date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EstimatedStartDate { get; set; }

        [DisplayName("Early bird entry deadline")]
        [Required(ErrorMessage = "Early bird entry deadline is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EntryDeadline { get; set; }

        [DisplayName("Contract From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "Contract From is required")]
        public DateTime? ContrctFrom { get; set; }

        [Required(ErrorMessage = "Contract To is required")]
        [DisplayName("Contract To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ContrctTo { get; set; }

        [Required(ErrorMessage = "Contract deliverables are required")]
        [DisplayName("Contract deliverables")]
        public string ContractDeliverables { get; set; }

        [DisplayName("Manufacturer")]
        [MaxLength(250)]
        public string Manufacturer { get; set; }

        public Int32? ManufacturerId { get; set; }

        [DisplayName("Website")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Website is required")]
        public string Website { get; set; }

        [DisplayName("Product Category")]
        [Required(ErrorMessage = "Product Category is required")]
        public int AEProductCategoryId { get; set; }

        [DisplayName("Product")]
        public int AEProductId { get; set; } // Required for active contract

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (ContrctFrom >= ContrctTo)
        //    {
        //        yield return new ValidationResult("ContrctTo must be greater than ContrctFrom");
        //    }
        //}
    }
}