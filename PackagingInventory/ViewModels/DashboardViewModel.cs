using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace PackagingInventory.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private int _totalBoxes;
        private decimal _totalPurchase;
        private decimal _totalSales;
        private decimal _totalPaymentReceived;
        private decimal _totalPaymentMade;
        private decimal _miscellaneousCost;
        private decimal _totalOutstandingPayment;
        private decimal _totalOutstandingReceivable;

        public int TotalBoxes
        {
            get => _totalBoxes;
            set { _totalBoxes = value; OnPropertyChanged(); }
        }

        public decimal TotalPurchase
        {
            get => _totalPurchase;
            set
            {
                if (_totalPurchase != value)
                {
                    _totalPurchase = value;
                    OnPropertyChanged();
                    CalculateOutstandingPayment();
                }
            }
        }

        public decimal TotalSales
        {
            get => _totalSales;
            set
            {
                if (_totalSales != value)
                {
                    _totalSales = value;
                    OnPropertyChanged();
                    CalculateOutstandingReceivable();
                }
            }
        }

        public decimal TotalPaymentReceived
        {
            get => _totalPaymentReceived;
            set
            {
                if (_totalPaymentReceived != value)
                {
                    _totalPaymentReceived = value;
                    OnPropertyChanged();
                    CalculateOutstandingReceivable();
                }
            }
        }

        public decimal TotalPaymentMade
        {
            get => _totalPaymentMade;
            set
            {
                if (_totalPaymentMade != value)
                {
                    _totalPaymentMade = value;
                    OnPropertyChanged();
                    CalculateOutstandingPayment();
                }
            }
        }

        public decimal MiscellaneousCost
        {
            get => _miscellaneousCost;
            set { _miscellaneousCost = value; OnPropertyChanged(); }
        }

        public decimal TotalOutstandingPayment
        {
            get => _totalOutstandingPayment;
            private set
            {
                if (_totalOutstandingPayment != value)
                {
                    _totalOutstandingPayment = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalOutstandingReceivable
        {
            get => _totalOutstandingReceivable;
            private set
            {
                if (_totalOutstandingReceivable != value)
                {
                    _totalOutstandingReceivable = value;
                    OnPropertyChanged();
                }
            }
        }


        public Dictionary<string, int> BoxesByType { get; set; } = new();

        private void CalculateOutstandingPayment()
        {
            TotalOutstandingPayment = TotalPurchase - TotalPaymentMade;
        }

        private void CalculateOutstandingReceivable()
        {
            TotalOutstandingReceivable = TotalSales - TotalPaymentReceived;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
