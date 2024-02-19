using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public class Income : FinanceObject
    {
        #region Constructors

        public Income(double amount)
            : base(amount, DateTime.Now)
        {

        }

        public Income(double amount, DateTime date)
            : base(amount, date)
        {

        }

        #endregion
    }
}
