using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using PackagingInventory.Controllers;
using PackagingInventory.Models;
using PackagingInventory.Services;

namespace PackagingInventory.Views
{
    public partial class GodownHistoryView : UserControl
    {
        private readonly DashboardController _controller = new DashboardController();
        private readonly ExcelService _excelService;
        public GodownHistoryView()
        {
            InitializeComponent();
            _excelService = new ExcelService();
            LoadGodownHistory();
        }

        private void LoadGodownHistory()
        {
            var boxesReceived = _excelService.GetBoxesReceived();
            var boxesSold = _excelService.GetBoxesSold();

            var combined = new List<dynamic>();

            combined.AddRange(boxesReceived.Select(x => new
            {
                Date = x.Date,
                Party = x.PartyName,
                Type = "Received",
                BoxTypes = string.Join(", ", x.BoxType.Select(bt => $"{bt.Key}: {bt.Value}")),
                Amount = x.Amount
            }));

            combined.AddRange(boxesSold.Select(x => new
            {
                Date = x.Date,
                Party = x.PartyName,
                Type = "Sold",
                BoxTypes = string.Join(", ", x.BoxType.Select(bt => $"{bt.Key}: {bt.Value}")),
                Amount = x.Amount
            }));

            GodownGrid.ItemsSource = combined.OrderByDescending(x => x.Date);
        }
    }
}
