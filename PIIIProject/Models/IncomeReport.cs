using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PIIIProject.Models
{
    public class IncomeReport : Report
    {
        #region Constructors

        public IncomeReport()
            : base(new List<FinanceObject>())
        {
            
        }

        public IncomeReport(List<Income> list)
            :base(list.Cast<FinanceObject>().ToList())
        {
            CalculateReport();
        }

        #endregion

        #region Methods
        //Updates the backing fields
        public override void CalculateReport()
        {
            base._total = CalculateTotal();
            base._average = CalculateAverage();
            base._median = CalculateMedian();
        }

        //Finds a file with incomes, and adds the incomes to the list and makes a report from it
        public override void GenerateReportFromFile(string filepath)
        {
            base._list.Clear();
            if (File.Exists(filepath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filepath);
                    foreach (string line in lines)
                    {
                        string[] splittedData = line.Split(',');
                        double amount = double.Parse(splittedData[0]);
                        DateTime date = DateTime.Parse(splittedData[1]);
                        base._list.Add(new Income(amount, date));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                throw new Exception("Error. File that was attempted to be loaded does not exist");
            }
            CalculateReport();
        }

        #endregion
    }
}
