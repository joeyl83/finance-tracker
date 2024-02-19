using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class FinanceObject
    {
        #region Backing fields

        private double _amount;
        private DateTime _date;

        #endregion

        #region Constructors

        public FinanceObject(double amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }

        #endregion

        #region Properties

        public double Amount
        {
            get { return _amount; }
            set
            {
                if (value < 0)
                    throw new Exception("Error. Amount cannot be negative");
                _amount = value;
            }
        }

        public string AmountToString
        {
            get { return Amount.ToString("C2"); }
        }

        public DateTime Date
        {
            get { return _date;  }
            set
            {
                if (value > DateTime.Now)
                    throw new Exception("Error. Date cannot be in the future");
                _date = value;
            }
        }

        public string MonthYearConcat
        {
            get { return Date.ToString("MMM") + " " + Date.Year; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return _amount.ToString() + "," + _date.ToString();
        }

        #endregion
    }
}
