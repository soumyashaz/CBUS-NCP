using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
    class ContractCentralRepository
    {

    }

    public class ContentSectionRepository
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
