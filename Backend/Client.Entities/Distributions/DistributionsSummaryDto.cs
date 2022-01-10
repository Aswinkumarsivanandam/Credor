using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class DistributionsSummaryDto
    {
        public string OfferingName { get; set; }
        public decimal Funded{ get; set; }
        public decimal OperatingIncome { get; set; }
        public decimal GainOnScale { get; set; }
        public decimal RefinanceProceeds { get; set; }
        public decimal ReturnOfCapital { get; set; }
        public decimal PreferredReturn { get; set; }
        public decimal Interest { get; set; }
        public decimal InvestmentBalance { get; set; }
    }
}
