using System;
using System.Collections.Generic;
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

namespace sGroupCalcWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        #region Private

        private double investment;
        private double averageProfit;
        private int numOfMonths;
        private double backofficeBalance;
        private DateTime endOfInvestment;
        private List<double> listOfBalancePerMonth;

        #endregion

        #region Public

        #endregion

        #endregion

        #region Methods

        #region CalculateBtnClick
        private void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            listOfBalancePerMonth = new List<double>();
            try
            {
                investment = Convert.ToDouble(investmentTextBox.Text);
            }
            catch(Exception ex)
            {
                investmentTextBox.Clear();
                Console.WriteLine(ex.Message);
                outputRtb.Document.Blocks.Clear();
                outputRtb.AppendText("Vnašaj samo cifre");
            }
            try
            {
                averageProfit = Convert.ToDouble(profitTextBox.Text);
            }
            catch(Exception ex)
            {
                profitTextBox.Clear();
                Console.WriteLine(ex.Message);
                outputRtb.Document.Blocks.Clear();
                outputRtb.AppendText("Vnašaj samo cifre");
            }
            endOfInvestment = withdrawalDate.SelectedDate.Value.Date;

            if (endOfInvestment < DateTime.Now)
            {
                outputRtb.Document.Blocks.Clear();
                outputRtb.AppendText("Timetraveler?");
                return;
            }
            
            if(investment < 100)
            {
                outputRtb.Document.Blocks.Clear();
                outputRtb.AppendText("Žal ne moreš investirati manj kot 100€.");
                return;
            }

            numOfMonths = ((endOfInvestment.Year - DateTime.Now.Year) * 12) + endOfInvestment.Month - DateTime.Now.Month;
            if(numOfMonths > 1200)
            {
                outputRtb.Document.Blocks.Clear();
                outputRtb.AppendText("Tk dougo pa mnda nboš živo xD");
                return;
            }

            for (int i=0;i<numOfMonths;i++)
            {
                if(investment >= 100 && investment < 500)
                {
                    investment = Calculate(investment, 0.5, ref averageProfit, ref backofficeBalance);
                }
                else if(investment >= 500 && investment < 1500)
                {
                    investment = Calculate(investment, 0.6, ref averageProfit, ref backofficeBalance);
                }
                else if(investment >= 1500 && investment < 5000)
                {
                    investment = Calculate(investment, 0.65, ref averageProfit, ref backofficeBalance);
                }
                else if(investment >= 5000 && investment < 10000)
                {
                    investment = Calculate(investment, 0.7, ref averageProfit, ref backofficeBalance);
                }
                else if(investment >= 10000 && investment < 25000)
                {
                    investment = Calculate(investment, 0.75, ref averageProfit, ref backofficeBalance);
                }
                else if(investment >= 25000 && investment < 50000)
                {
                    investment = Calculate(investment, 0.8, ref averageProfit, ref backofficeBalance);
                }
                else if(investment >= 50000)
                {
                    investment = Calculate(investment, 0.85, ref averageProfit, ref backofficeBalance);
                }
                listOfBalancePerMonth.Add(investment);
            }

            outputRtb.Document.Blocks.Clear();
            Print(listOfBalancePerMonth);
        }
        #endregion

        #region Calculate
        private double Calculate(double investment, double percent, ref double averageProfit, ref double backofficeBalance)
        {
            double profit = 0;
            profit = (investment * averageProfit/100) * percent;
            if (profit < 15)
            {
                backofficeBalance += profit;
            }
            else
            {
                investment = investment + (investment * averageProfit/100) * percent * 0.95;
            }
            if (backofficeBalance >= 105)
            {
                investment += backofficeBalance * 0.95;
                backofficeBalance = 0;
            }

            return investment;
        }
        #endregion

        #region Print
        private void Print(List<double> listOfBalancePerMonth)
        {
            for(int i = 0; i < listOfBalancePerMonth.Count; i++)
            {
                if(i==0)
                {
                    outputRtb.AppendText((i+1).ToString() + " - mesec -> Profit: " + Math.Round(listOfBalancePerMonth[i]-listOfBalancePerMonth[i], 2).ToString() + " -> Balance: " + Math.Round(listOfBalancePerMonth[i],2).ToString()+"\n");
                }
                else
                {
                    outputRtb.AppendText("\n"+(i+1).ToString() + " - mesec -> Profit: " + Math.Round((listOfBalancePerMonth[i] - listOfBalancePerMonth[i - 1]),2).ToString() + " -> Balance: " + Math.Round(listOfBalancePerMonth[i],2).ToString());
                }
            }
        }
        #endregion

        #endregion
    }
}
