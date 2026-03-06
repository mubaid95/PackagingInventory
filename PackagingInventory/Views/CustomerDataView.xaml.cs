using PackagingInventory.Controllers;
using PackagingInventory.Models;
using PackagingInventory.Services;
using System;
using System.Linq;
using System.Windows.Controls;

namespace PackagingInventory.Views
{
    public partial class CustomerDataView : UserControl
    {
        private readonly DashboardController _controller = new DashboardController();
        private readonly ExcelService _excelService;
        public CustomerDataView()
        {
            InitializeComponent();
            _excelService = new ExcelService();
            LoadCustomerDropdown();
        }

        private void LoadCustomerDropdown()
        {
            // Load customer names from BoxesSold sheet
            var soldRecords = _excelService.GetBoxesSold();
            var customerNames = soldRecords.Select(x => x.PartyName).Distinct().OrderBy(x => x).ToList();

            CustomerDropdown.ItemsSource = customerNames;
        }

        private void CustomerDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerDropdown.SelectedItem == null)
                return;

            string selectedCustomer = CustomerDropdown.SelectedItem.ToString();

            LoadCustomerData(selectedCustomer);
        }

        private void LoadCustomerData(string customer)
        {
            var soldRecords = _excelService.GetBoxesSold()
                .Where(x => x.PartyName.Equals(customer, StringComparison.OrdinalIgnoreCase))
                .ToList();

            int totalBoxesSold = soldRecords
                .SelectMany(x => x.BoxType)
                .Sum(bt => bt.Value);

            var boxesByType = soldRecords
                .SelectMany(x => x.BoxType)
                .GroupBy(bt => bt.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(bt => bt.Value)
                );


            var paymentRecords = _excelService.GetPaymentsReceived()
                .Where(x => x.PartyName.Equals(customer, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var soldDisplay = soldRecords.Select(x => new
            {
                Date = x.Date.ToShortDateString(),
                BoxTypes = string.Join(", ", x.BoxType.Select(bt => $"{bt.Key}: {bt.Value}")),
                Amount = x.Amount
            }).ToList();

            SalesGrid.ItemsSource = soldDisplay;
            PaymentGrid.ItemsSource = paymentRecords;
            BoxesSoldByTypePanel.ItemsSource = boxesByType;

            decimal totalSale = soldRecords.Sum(x => x.Amount);
            decimal totalPayment = paymentRecords.Sum(x => x.Amount);
            decimal outstanding = totalSale - totalPayment;

            TotalSaleText.Text = totalSale.ToString("C");
            PaymentReceivedText.Text = totalPayment.ToString("C");
            OutstandingText.Text = outstanding.ToString("C");
            TotalBoxesSoldText.Text = totalBoxesSold.ToString("C");
        }
    }
}
