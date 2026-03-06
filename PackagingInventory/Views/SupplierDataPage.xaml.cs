using PackagingInventory.Controllers;
using PackagingInventory.Models;
using PackagingInventory.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PackagingInventory.Views
{
    public partial class SupplierDataPage : UserControl
    {
        private readonly ExcelService _excelService;

        public SupplierDataPage()
        {
            InitializeComponent();
            _excelService = new ExcelService();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            var suppliers = _excelService.GetBoxesReceived()
                                         .Select(s => s.PartyName)
                                         .Distinct()
                                         .OrderBy(n => n)
                                         .ToList();

            SupplierComboBox.ItemsSource = suppliers;
        }

        private void SupplierComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SupplierComboBox.SelectedItem == null) return;

            string supplier = SupplierComboBox.SelectedItem.ToString();
            LoadSupplierData(supplier);
        }

        private void LoadSupplierData(string supplier)
        {

            var rawPurchases = _excelService.GetBoxesReceived()
                                    .Where(x => x.PartyName == supplier)
                                    .ToList();
            // Load purchase history
            var purchases = rawPurchases
                            .Select(x => new
                            {
                                Date = x.Date.ToShortDateString(),
                                BoxTypeDisplay = string.Join(", ", x.BoxType.Select(b => $"{b.Key}: {b.Value}")),
                                x.Amount
                            })
                            .ToList();

            PurchaseHistoryGrid.ItemsSource = purchases;

            // Load payment history
            var payments = _excelService.GetPaymentsMade()
                                        .Where(p => p.PartyName == supplier)
                                        .OrderBy(p => p.Date)
                                        .ToList();

            PaymentHistoryGrid.ItemsSource = payments;

            int totalBoxesPurchased = rawPurchases
                              .SelectMany(x => x.BoxType)
                              .Sum(bt => bt.Value);

            TotalBoxesPurchasedText.Text = totalBoxesPurchased.ToString();

            var boxesByType = rawPurchases
                              .SelectMany(x => x.BoxType)
                              .GroupBy(bt => bt.Key)
                              .ToDictionary(g => g.Key, g => g.Sum(bt => bt.Value));

            BoxesPurchasedByTypePanel.ItemsSource = boxesByType;

            // Summary
            decimal totalPurchase = rawPurchases.Sum(x => x.Amount);
            decimal totalPaid = payments.Sum(x => x.Amount);
            decimal outstanding = totalPurchase - totalPaid;

            TotalPurchaseText.Text = totalPurchase.ToString("C");
            TotalPaymentText.Text = totalPaid.ToString("C");
            OutstandingText.Text = outstanding.ToString("C");
        }
    }
}
