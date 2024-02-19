using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PIIIProject.Models;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FinanceTracker tracker = new FinanceTracker();

        ObservableCollection<Expense> expenses;

        ObservableCollection<Income> incomes;



        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Finance Tracker";

            //Gets the collection from the list of all expenses
            expenses = new ObservableCollection<Expense>(tracker.AllExpensesList);
            incomes = new ObservableCollection<Income>(tracker.AllIncomesList);

            //Adds the values to the table on the interface
            ExpensesTable.ItemsSource = expenses;
            IncomeTable.ItemsSource = incomes;

            //Display the totals on the screen
            IncomeBalance.Text += tracker.TotalIncome.ToString("C2");
            ExpensesBalance.Text += tracker.TotalExpenses.ToString("C2");

            TotalBalance.Text += (tracker.TotalIncome - tracker.TotalExpenses).ToString("C2");

            CheckForNegativeBalance();
        }

        private void BtnSubmitIncome_Click(object sender, RoutedEventArgs e)
        {
            //Checks the data:
            if (!double.TryParse(txbBalance1.Text, out double amount))
            {
                MessageBox.Show("Error. Invalid Amount", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (double.Parse(txbBalance1.Text) <= 0)
            {
                MessageBox.Show("Error. Cannot Input Negative Value", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Income income = new Income(double.Parse(txbBalance1.Text));

            tracker.AddIncome(income);

            //Adds it to the table
            incomes.Add(income);

            //Adds it to the totals
            IncomeBalance.Text = "Income Balance: " + tracker.TotalIncome.ToString("C2");

            TotalBalance.Text = "Total Balance: " + (tracker.TotalIncome - tracker.TotalExpenses).ToString("C2");

            MessageBox.Show("Income added!", "Data added", MessageBoxButton.OK, MessageBoxImage.Information);

            txbBalance1.Text = "";

            CheckForNegativeBalance();
        }

        private void BtnSubmitExpense_Click(object sender, RoutedEventArgs e)
        {
            //Checks data:
            if (!double.TryParse(txbBalance2.Text, out double amount))
            {
                MessageBox.Show("Error. Invalid Amount", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string cmbCat = "";

            if(cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                cmbCat = (cmbCategory.SelectedItem as ComboBoxItem).Content.ToString();
            }           

            if (double.Parse(txbBalance2.Text) <= 0)
            {
                MessageBox.Show("Error. Cannot Input Negative Value", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Expense expense = new Expense(double.Parse(txbBalance2.Text), DateTime.Now, cmbCat);

            tracker.AddExpense(expense);

            expenses.Add(expense);

            ExpensesBalance.Text = "Expenses Balance: " + tracker.TotalExpenses.ToString("C2");

            TotalBalance.Text = "Total Balance: " + (tracker.TotalIncome - tracker.TotalExpenses).ToString("C2");

            MessageBox.Show("Expense added!", "Data added", MessageBoxButton.OK, MessageBoxImage.Information);

            txbBalance2.Text = "";

            CheckForNegativeBalance();
        }

        private void BtnLoadIncomeReport(object sender, RoutedEventArgs e)
        {
            if(tracker.LoadedIncomesList.Count == 0)
            {
                MessageBox.Show("Error. Cannot make report without any data", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IncomeReport report = new IncomeReport(tracker.LoadedIncomesList);
                tracker.LoadIncomeReport(report);
            }
        }

        private void BtnLoadExpenseReport(object sender, RoutedEventArgs e)
        {
            if(tracker.LoadedExpensesList.Count == 0)
            {
                MessageBox.Show("Error. Cannot make report without any data", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ExpenseReport report = new ExpenseReport(tracker.LoadedExpensesList);
                tracker.LoadExpenseReport(report);
            }
        }

        private void BtnLoadMonthlyData(object sender, RoutedEventArgs e)
        {
            try
            {
                //Gets the month and year
                int year;
                int month;
                if(!int.TryParse(txbIncomeMonth.Text, out month))
                {
                    throw new Exception("Error. Invalid input for month");
                }
                if(!int.TryParse(txbIncomeYear.Text, out year))
                {
                    throw new Exception("Error. Invalid input for year");
                }

                //Loads from the appropriate files
                string expenseFilePath = FinanceTracker.EXPENSES_MONTH_PATH + "expenses-" + month + "-" + year + ".txt";
                string incomeFilePath = FinanceTracker.INCOME_MONTH_PATH + "income-" + month + "-" + year + ".txt";
                tracker.LoadExpenseFile(expenseFilePath);
                tracker.LoadIncomeFile(incomeFilePath);

                //Redo the tables with only the loaded data
                expenses = new ObservableCollection<Expense>(tracker.LoadedExpensesList);
                incomes = new ObservableCollection<Income>(tracker.LoadedIncomesList);


                ExpensesTable.ItemsSource = expenses;
                IncomeTable.ItemsSource = incomes;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLoadAllData(object sender, RoutedEventArgs e)
        {
            tracker.LoadExpenseFile(FinanceTracker.ALL_EXPENSES_FILE_PATH);
            tracker.LoadIncomeFile(FinanceTracker.ALL_INCOMES_FILE_PATH);

            expenses = new ObservableCollection<Expense>(tracker.AllExpensesList);
            incomes = new ObservableCollection<Income>(tracker.AllIncomesList);

            ExpensesTable.ItemsSource = expenses;
            IncomeTable.ItemsSource = incomes;
        }

        private void CheckForNegativeBalance()
        {
            if(tracker.TotalExpenses > tracker.TotalIncome)
            {
                MessageBox.Show("WARNING. Expenses are surpassing income.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
