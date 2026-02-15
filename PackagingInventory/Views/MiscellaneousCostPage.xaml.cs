using PackagingInventory.Services;
using System.Linq;
using System.Windows.Controls;

namespace PackagingInventory.Views
{
    public partial class MiscellaneousCostPage : UserControl
    {
        private readonly ExcelService _excelService;

        public MiscellaneousCostPage()
        {
            InitializeComponent();
            _excelService = new ExcelService();
            LoadMiscCosts();
        }

        private void LoadMiscCosts()
        {
            var miscList = _excelService.GetMiscCosts();

            MiscCostGrid.ItemsSource = miscList;

            decimal total = miscList.Sum(x => x.Amount);
            TotalMiscCostText.Text = total.ToString("C");
        }
    }
}
