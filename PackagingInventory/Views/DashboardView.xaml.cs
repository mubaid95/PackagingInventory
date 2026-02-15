using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PackagingInventory.ViewModels;
using PackagingInventory.Controllers;

namespace PackagingInventory.Views
{
    public partial class DashboardView : UserControl
    {
        private readonly DashboardController _controller = new DashboardController();
        private readonly DashboardViewModel _viewModel = new DashboardViewModel();

        public DashboardView()
        {
            InitializeComponent();
            LoadDashboardData();
            DataContext = _viewModel;
        }

        private void LoadDashboardData()
        {
            var boxTotals = _controller.GetBoxTotals();
            var purchase = _controller.GetPurchaseSummary();
            var sales = _controller.GetSalesSummary();
            var misc = _controller.GetMiscellaneousSummary();

            _viewModel.TotalBoxes = boxTotals.totalBoxesRemaining;
            _viewModel.TotalPurchase = purchase.totalPurchase;
            _viewModel.TotalSales = sales.totalSales;
            _viewModel.TotalPaymentMade = purchase.paidPurchase;
            _viewModel.TotalPaymentReceived = sales.receivedSales;
            _viewModel.MiscellaneousCost = misc;
            _viewModel.BoxesByType = boxTotals.remainingByType;
        }
    }
}
