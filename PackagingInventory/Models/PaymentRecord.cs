using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagingInventory.Models
{
    public class PaymentRecord
    {
        public DateTime Date { get; set; }
        public string PartyName { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; }
    }
}
