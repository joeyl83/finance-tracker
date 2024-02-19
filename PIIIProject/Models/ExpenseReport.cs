using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class ExpenseReport : Report
    {
        #region Backing Fields

        private string _mostUsedCategory;

        #endregion

        #region Constructors

        public ExpenseReport()
            :base(new List<FinanceObject>())
        {
            _mostUsedCategory = string.Empty;
        }

        public ExpenseReport(List<Expense> list)
            :base(list.Cast<FinanceObject>().ToList())
        {
            CalculateReport();
        }

        #endregion

        #region Properties

        public string MostUsedCategory
        {
            get { return _mostUsedCategory; }
        }

        #endregion

        #region Methods

        public string CalculateMostUsedCategory()
        {
            //new list with one of every category present in the list
            List<string> categoryList = new List<String>();

            //generates the new list
            foreach(Expense e in _list)
            {
                bool unique = true;
                foreach(string category in categoryList)
                {
                    if(e.Category == category)
                    {
                        unique = false;
                        break;
                    }    
                }
                if (unique)
                {
                    categoryList.Add(e.Category);
                }
            }

            //Gets the highest count and the index of a category in the list of expenses
            int maxCount = 0;
            int maxCountIndex = 0;
            for(int i = 0; i < categoryList.Count; i++)
            {
                int count = 0;
                foreach(Expense e in _list)
                {
                    if(e.Category == categoryList[i])
                    {
                        count++;
                    }
                }

                if(count > maxCount)
                {
                    maxCount = count;
                    maxCountIndex = i;
                }
            }

            return categoryList[maxCountIndex];
        }

        public override void CalculateReport()
        {
            _total = CalculateTotal();
            _average = CalculateAverage();
            _median = CalculateMedian();
            _mostUsedCategory = CalculateMostUsedCategory();
        }

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
                        string category = splittedData[2];
                        base._list.Add(new Expense(amount, date, category));
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

        public override string ToString()
        {
            return $"Total: {Total}" +
                $"\nAverage: {Average}" +
                $"\nMedian: {Median}" +
                $"\nMost used category: {MostUsedCategory}";
        }

        #endregion
    }
}
