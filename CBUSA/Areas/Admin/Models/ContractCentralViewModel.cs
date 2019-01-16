using CBUSA.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Areas.Admin.Models
{
    public class ContractCentralViewModel
    {

    }

    public class ContentSectionViewModel
    {
        public Int64 SectionId { get; set; }
        public string DisplayValue { get; set; }
        public string SortValue { get; set; }
        public byte AboveBar { get; set; }
        public string ToolTip { get; set; }
        public string Icon { get; set; }
        public bool InternalOnly { get; set; }
        public bool JoinedOnly { get; set; }
        public bool AutoAdd { get; set; }
        public int ContentCount { get; set; }
    }
}