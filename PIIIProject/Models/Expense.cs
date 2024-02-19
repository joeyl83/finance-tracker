using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Expense : FinanceObject
    {
        #region BackingFields

        const string DEFAULT_CATEGORY = "not-specified";
        private string _category;

        #endregion

        #region Constructors

        public Expense(double amount)
            :base(amount, DateTime.Now)
        {
            Category = DEFAULT_CATEGORY;
        }

        public Expense(double amount, DateTime date, string category)
            :base(amount, date)
        {
            Category = category;
        }

        #endregion

        #region Properties

        public string Category
        {
            get { return _category; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Error. Category cannot be null or empty");
                _category = value;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return base.Amount.ToString()+ "," + base.Date.ToString()+ "," + Category;
        }

        #endregion
    }
}
