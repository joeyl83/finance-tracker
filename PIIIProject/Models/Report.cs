using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public abstract class Report
    {
        #region Backing Fields

        protected double _total;
        protected double _average;
        protected double _median;
        protected List<FinanceObject> _list;

        #endregion

        #region Constructors

        public Report(List<FinanceObject> list)
        {
            _list = list;
            _total = 0;
            _average = 0;
            _median = 0;
        }

        #endregion

        #region Properties

        public double Total
        {
            get { return _total; }
        }

        public double Average
        {
            get { return _average; }
        }

        public double Median
        {
            get { return _median; }
        }

        public List<FinanceObject> List
        {
            get { return _list; }
            set
            {
                _list = value;
            }
        }

        #endregion

        #region Methods

        public double CalculateTotal()
        {
            double total = 0;
            foreach (FinanceObject f in _list)
            {
                total += f.Amount;
            }
            _total = total;
            return _total;
        }

        public double CalculateAverage()
        {
            double total = 0;
            foreach (FinanceObject f in _list)
            {
                total += f.Amount;
            }

            _average = total / _list.Count;
            return _average;
        }

        public double CalculateMedian()
        {
            List<FinanceObject> listCopy = new List<FinanceObject>();
            listCopy = _list.OrderBy(x => x.Amount).ToList();

            if (listCopy.Count % 2 == 0)
            {
                _median = (listCopy[listCopy.Count / 2].Amount + listCopy[(listCopy.Count / 2) - 1].Amount) / 2;
            }
            else
            {
                _median = listCopy[listCopy.Count / 2].Amount;
            }
            return _median;
        }

        public void AddToList(FinanceObject f)
        {
            _list.Add(f);
        }

        public void RemoveFromList(int index)
        {
            _list.RemoveAt(index);
        }

        public void SaveReport(string filepath)
        {
            StreamWriter sw = new StreamWriter(filepath, true);
            sw.WriteLine(ToString());
            sw.Close();
        }

        public override string ToString()
        {
            return $"Total: {Total.ToString("C2")}" +
                $"\nAverage: {Average.ToString("C2")}" +
                $"\nMedian: {Median.ToString("C2")}";
        }

        abstract public void GenerateReportFromFile(string filePath);
        abstract public void CalculateReport();

        #endregion
    }
}
