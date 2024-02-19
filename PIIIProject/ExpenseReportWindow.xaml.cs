using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using PIIIProject.Models;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for ExpenseReportWindow.xaml
    /// </summary>
    public partial class ExpenseReportWindow : Window
    {
        private ExpenseReport _report;
        public ExpenseReportWindow(ExpenseReport report)
        {
            InitializeComponent();
            _report = report;
            ReportText.Content = report.ToString();
        }

        private void BtnSaveExpenseReport(object sender, RoutedEventArgs e)
        {
            //Makes file name with date and time and saves the report
            string month = DateTime.Now.ToString("MM");
            string year = DateTime.Now.Year.ToString();
            string day = DateTime.Now.ToString("dd");
            string time = DateTime.Now.ToString("HH-mm-ss");
            string filePath = FinanceTracker.EXPENSES_REPORT_PATH + "expense-report-" + year + "-" + month + "-" + day + "-" + time + ".txt";
            StreamWriter sw = new StreamWriter(filePath);
            string content = _report.ToString();
            sw.WriteLine(content);
            sw.Close();
            MessageBox.Show("Report saved successfuly", "Report Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
