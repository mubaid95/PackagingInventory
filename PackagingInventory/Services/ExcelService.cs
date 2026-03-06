using ClosedXML.Excel;
using PackagingInventory.Helpers;
using PackagingInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagingInventory.Services
{
    public class ExcelService
    {
        public List<BoxTransaction> GetBoxesReceived()
        {
            return ReadBoxTransactions(FilePathHelper.BoxesReceivedPath);
        }

        public List<BoxTransaction> GetBoxesSold()
        {
            return ReadBoxTransactions(FilePathHelper.BoxesSoldPath);
        }

        public List<PaymentRecord> GetPaymentsReceived()
        {
            return ReadPaymentRecords(FilePathHelper.PaymentReceivedPath);
        }

        public List<PaymentRecord> GetPaymentsMade()
        {
            return ReadPaymentRecords(FilePathHelper.PaymentMadePath);
        }

        public List<MiscCost> GetMiscCosts()
        {
            var result = new List<MiscCost>();
            using var wb = new XLWorkbook(FilePathHelper.MiscCostPath);
            var ws = wb.Worksheet(1);

            foreach (var row in ws.RowsUsed().Skip(1))
            {
                result.Add(new MiscCost
                {
                    Date = row.Cell(1).GetDateTime(),
                    Description = row.Cell(2).GetString(),
                    Amount = (decimal)row.Cell(3).GetDouble()
                });
            }

            return result;
        }

        private List<BoxTransaction> ReadBoxTransactions(string path)
        {
            var result = new List<BoxTransaction>();
            using var wb = new XLWorkbook(path);
            var ws = wb.Worksheet(1);
            var boxTypeHeaders = ws.Row(1)
                           .Cells(3, 13)
                           .Select(c => c.GetString().Trim())
                           .ToList();
            foreach (var row in ws.RowsUsed().Skip(1))
            {
                var boxDict = new Dictionary<string, int>();

                for (int i = 0; i < boxTypeHeaders.Count; i++)
                {
                    string typeName = boxTypeHeaders[i];
                    var cellValue = row.Cell(i + 3); // Because 3–11

                    int qty = 0;
                    if (!cellValue.IsEmpty())
                    {
                        double val;
                        if (double.TryParse(cellValue.GetValue<string>(), out val))
                        {
                            qty = (int)val;
                        }
                    }

                    boxDict[typeName] = qty;
                }

                result.Add(new BoxTransaction
                {
                    Date = row.Cell(1).GetDateTime(),
                    PartyName = row.Cell(2).GetString(),
                    BoxType = boxDict,
                    Amount = (decimal)row.Cell(14).GetDouble() // Assuming column 12 has Amount
                });
            }

            return result;
        }

        private List<PaymentRecord> ReadPaymentRecords(string path)
        {
            var result = new List<PaymentRecord>();
            using var wb = new XLWorkbook(path);
            var ws = wb.Worksheet(1);

            foreach (var row in ws.RowsUsed().Skip(1))
            {
                result.Add(new PaymentRecord
                {
                    Date = row.Cell(1).GetDateTime(),
                    PartyName = row.Cell(2).GetString(),
                    Amount = (decimal)row.Cell(3).GetDouble(),
                    Mode = row.Cell(4).GetString()
                });
            }

            return result;
        }
    }
}
