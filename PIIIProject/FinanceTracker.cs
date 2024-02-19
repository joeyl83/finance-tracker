using PIIIProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject
{
    //Class that manages all the data in the app behind the scenes
    internal class FinanceTracker
    {
        #region Backing Fields

        private List<Expense> _allExpensesList;
        private List<Income> _allIncomesList;
        public const string ALL_EXPENSES_FILE_PATH = "./data/expenses/allExpenses.txt";
        public const string ALL_INCOMES_FILE_PATH = "./data/incomes/allIncomes.txt";
        private double _totalExpenses;
        private double _totalIncome;
        private List<Expense> _loadedExpensesList;
        private List<Income> _loadedIncomesList;
        public const string INCOME_MONTH_PATH = "./data/incomes/monthly/";
        public const string EXPENSES_MONTH_PATH = "./data/expenses/monthly/";
        public const string INCOME_REPORT_PATH = "./data/incomes/reports/";
        public const string EXPENSES_REPORT_PATH = "./data/expenses/reports/";

        #endregion

        #region Constructors

        public FinanceTracker()
        {
            //Initialize the lists
            _loadedIncomesList = new List<Income>();
            _loadedExpensesList = new List<Expense>();

            //Make directories / files if they don't exist

            if(!Directory.Exists("./data"))
            {
                Directory.CreateDirectory("./data");
            }
            if (!Directory.Exists("./data/incomes"))
            {
                Directory.CreateDirectory("./data/incomes");
            }
            if (!Directory.Exists("./data/expenses"))
            {
                Directory.CreateDirectory("./data/expenses");
            }
            if (!Directory.Exists("./data/incomes/monthly"))
            {
                Directory.CreateDirectory("./data/incomes/monthly");
            }
            if (!Directory.Exists("./data/incomes/reports"))
            {
                Directory.CreateDirectory("./data/incomes/reports");
            }
            if (!Directory.Exists("./data/expenses/monthly"))
            {
                Directory.CreateDirectory("./data/expenses/monthly");
            }
            if (!Directory.Exists("./data/expenses/reports"))
            {
                Directory.CreateDirectory("./data/expenses/reports");
            }

            if (!File.Exists(ALL_EXPENSES_FILE_PATH))
            {
                File.Create(ALL_EXPENSES_FILE_PATH);
            }
            if (!File.Exists(ALL_INCOMES_FILE_PATH))
                {
                File.Create(ALL_INCOMES_FILE_PATH);
            }

            //Load all the expenses
            _allExpensesList = LoadExpenseFile(ALL_EXPENSES_FILE_PATH);
            _allIncomesList = LoadIncomeFile(ALL_INCOMES_FILE_PATH);

            //Add the totals
            foreach (Expense e in _allExpensesList)
            {
                _totalExpenses += e.Amount;
            }

            foreach (Income i in _allIncomesList)
            {
                _totalIncome += i.Amount;
            }
        }

        #endregion

        #region Properties

        public double TotalExpenses
        {
            get { return _totalExpenses; }
        }

        public double TotalIncome
        {
            get { return _totalIncome; }
        }

        public List<Expense> AllExpensesList
        {
            get { return _allExpensesList; }
        }

        public List<Income> AllIncomesList
        {
            get { return _allIncomesList; }
        }

        public List<Expense> LoadedExpensesList
        {
            get { return _loadedExpensesList; }
        }

        public List<Income> LoadedIncomesList
        {
            get { return _loadedIncomesList; }
        }

        #endregion

        #region Methods

        //Gets a list of incomes from a file with data on multiple incomes
        public List<Income> LoadIncomeFile(string filePath)
        {
            _loadedIncomesList.Clear();
            if(File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach(string line in lines)
                    {
                        string[] splittedData = line.Split(',');
                        double amount = double.Parse(splittedData[0]);
                        DateTime date = DateTime.Parse(splittedData[1]);
                        _loadedIncomesList.Add(new Income(amount, date));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                throw new Exception("Error. File that was attempted to be loaded does not exist");
            }
            return _loadedIncomesList;
        }

        //Gets a list of expenses from a file with data on multiple expenses
        public List<Expense> LoadExpenseFile(string filePath)
        {
            _loadedExpensesList.Clear();
            if (File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] splittedData = line.Split(',');
                        double amount = double.Parse(splittedData[0]);
                        DateTime date = DateTime.Parse(splittedData[1]);
                        string category = splittedData[2];
                        _loadedExpensesList.Add(new Expense(amount, date, category));
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
            return _loadedExpensesList;
        }

        //Save loaded list of incomes into file form
        public void SaveLoadedIncomeFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Income i in _loadedIncomesList)
            {
                sb.AppendLine(i.ToString());
            }
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine(sb.ToString());
            sw.Close();
        }

        //Save loaded list of expenses into file form
        public void SaveLoadedExpenseFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Expense e in _loadedExpensesList)
            {
                sb.AppendLine(e.ToString());
            }
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine(sb.ToString());
            sw.Close();
        }

        public void AddIncome(Income i)
        {
            _loadedIncomesList.Add(i);
            StreamWriter sw = new StreamWriter(ALL_INCOMES_FILE_PATH, true);
            sw.WriteLine(i.ToString());
            sw.Close();

            StreamWriter sw2 = new StreamWriter(GetIncomeMonthFilePath(i), true);
            sw2.WriteLine(i.ToString());
            sw2.Close();

            _totalIncome += i.Amount;
        }

        public void AddExpense(Expense e)
        {
            _loadedExpensesList.Add(e);
            StreamWriter sw = new StreamWriter(ALL_EXPENSES_FILE_PATH, true);
            sw.WriteLine(e.ToString());
            sw.Close();

            StreamWriter sw2 = new StreamWriter(GetExpenseMonthFilePath(e), true);
            sw2.WriteLine(e.ToString());
            sw2.Close();

            _totalExpenses += e.Amount;
        }

        //Makes the month file path that an income object will be added to depending on the date of the income
        private string GetIncomeMonthFilePath(Income i)
        {
            string month = i.Date.Month.ToString();
            string year = i.Date.Year.ToString();
            return INCOME_MONTH_PATH + "income" + "-" + month + "-" + year + ".txt";
        }

        //Makes the month file path that an expense object will be added to depending on the date of the expense
        private string GetExpenseMonthFilePath(Expense e)
        {
            string month = e.Date.Month.ToString();
            string year = e.Date.Year.ToString();
            return EXPENSES_MONTH_PATH + "expenses" + "-" + month + "-" + year + ".txt";
        }

        public void LoadExpenseReport(ExpenseReport e)
        {
            ExpenseReportWindow reportDisplay = new ExpenseReportWindow(e);
            reportDisplay.Show();
        }

        public void LoadIncomeReport(IncomeReport i)
        {
            IncomeReportWindow reportDisplay = new IncomeReportWindow(i);
            reportDisplay.Show();
        }

        public void ClearLoadedIncomes()
        {
            _loadedIncomesList.Clear();
        }

        public void ClearLoadedExpenses()
        {
            _loadedExpensesList.Clear();
        }

        #endregion
    }
}
