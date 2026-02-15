using PackagingInventory.Models;
using PackagingInventory.Services;
using System.Collections.Generic;
using System.Linq;

namespace PackagingInventory.Controllers
{
    public class DashboardController
    {
        private readonly ExcelService _excelService;

        public DashboardController()
        {
            _excelService = new ExcelService();
        }

        // 📦 Get total boxes by type and overall remaining count
        public (Dictionary<string, int> boxesReceivedByType,
                Dictionary<string, int> boxesSoldByType,
                Dictionary<string, int> remainingByType,
                int totalBoxesRemaining) GetBoxTotals()
        {
            var received = _excelService.GetBoxesReceived();
            var sold = _excelService.GetBoxesSold();

            // Sum up received boxes by type
            var receivedByType = received
                .SelectMany(r => r.BoxType)
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));

            // Sum up sold boxes by type
            var soldByType = sold
                .SelectMany(s => s.BoxType)
                .GroupBy(x => x.Key)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));

            // Calculate remaining boxes by type
            var allTypes = receivedByType.Keys.Union(soldByType.Keys);
            var remainingByType = allTypes.ToDictionary(
                type => type,
                type =>
                {
                    int receivedQty = receivedByType.ContainsKey(type) ? receivedByType[type] : 0;
                    int soldQty = soldByType.ContainsKey(type) ? soldByType[type] : 0;
                    return receivedQty - soldQty;
                });

            // Total remaining boxes across all types
            int totalRemaining = remainingByType.Values.Sum();

            return (receivedByType, soldByType, remainingByType, totalRemaining);
        }

        // 💰 Supplier Summary (Purchase)
        public (decimal totalPurchase, decimal paidPurchase, decimal pendingPurchase) GetPurchaseSummary()
        {
            var received = _excelService.GetBoxesReceived();
            var payments = _excelService.GetPaymentsMade();

            decimal total = received.Sum(x => x.Amount);
            decimal paid = payments.Sum(x => x.Amount);
            decimal pending = total - paid;

            return (total, paid, pending);
        }

        // 💸 Customer Summary (Sales)
        public (decimal totalSales, decimal receivedSales, decimal outstanding) GetSalesSummary()
        {
            var sold = _excelService.GetBoxesSold();
            var payments = _excelService.GetPaymentsReceived();

            decimal total = sold.Sum(x => x.Amount);
            decimal receivedAmt = payments.Sum(x => x.Amount);
            decimal outstanding = total - receivedAmt;

            return (total, receivedAmt, outstanding);
        }

        public decimal GetMiscellaneousSummary()
        {
            var misc = _excelService.GetMiscCosts();

            decimal totmisc = misc.Sum(x => x.Amount);

            return totmisc;
        }
    }
}
