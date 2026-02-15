using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagingInventory.Models
{
    internal class SummaryModels
    {
        public class CustomerSummary
        {
            public string CustomerName { get; set; }
            public Dictionary<string,int> TotalBoxesSold { get; set; }
            public decimal TotalSaleValue { get; set; }
            public decimal TotalPaymentReceived { get; set; }
            public decimal Outstanding => TotalSaleValue - TotalPaymentReceived;
        }

        public class SupplierSummary
        {
            public string SupplierName { get; set; }
            public Dictionary<string, int> TotalBoxesBought { get; set; }
            public decimal TotalPurchaseValue { get; set; }
            public decimal TotalPaymentMade { get; set; }
            public decimal Outstanding => TotalPurchaseValue - TotalPaymentMade;
        }
    }
}
