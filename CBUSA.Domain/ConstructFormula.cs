using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ConstructFormula : BaseColumnField
    {
        public Int64 ConstructFormulaId { get; set; }
        [Required]
        public Int64 ContractId { get; set; }
        [Required]
        public Int64 SurveyId { get; set; }
        [Required]
        public string Quarter { get; set; }
        public string Year { get; set; }
        //[Required]
        //public Int64 MarketId { get; set; }
        //[Required]
        //public Int64 QuestionId { get; set; }
        [Required]
        public Int64 QuestionColumnId { get; set; }
        public Int64 QuestionColumnValueId { get; set; }
        public string FormulaDescription { get; set; }
        public string FormulaBuild { get; set; }
        //   public Int32 QuestionTypeId { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Survey Survey { get; set; }
        //   public virtual Question Question { get; set; }
        // public virtual Market Market { get; set; }
        public virtual Quater Quater { get; set; }
        public ICollection<ConstructFormulaMarket> ConstructFormulaMarket { get; set; }
    }

    public class ConstructFormulaMarket
    {
        public Int64 ConstructFormulaMarketId { get; set; }
        public Int64 ConstructFormulaId { get; set; }
        public Int64 MarketId { get; set; }
        public virtual ConstructFormula ConstructFormula { get; set; }
        public virtual Market Market { get; set; }

    }

}
