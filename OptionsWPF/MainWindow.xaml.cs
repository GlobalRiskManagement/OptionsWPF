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
using MahApps.Metro.Controls;
using ValueAtRisk.Calculations;
using ValueAtRisk.Models.Inputs;
using ValueAtRisk.Models.Instruments;


namespace OptionsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            OptionCalculator optionCalc = new OptionCalculator();
        }

        private void rbAsian_Checked(object sender, RoutedEventArgs e)
        {
            groupBoxAsian.IsEnabled = true;
            groupBoxEuropean.IsEnabled = false;
            if (groupBoxEuropean.IsEnabled == false)
            {
                txtAssetPrice.Text = "";
                txtRiskFreeRate.Text = "";
                txtVolatility.Text = "";
                endDate.SelectedDate = null;
                endDate.Text = "";
                valuationDate.SelectedDate = null;
                valuationDate.Text = "";
                if (!string.IsNullOrEmpty(txtCostOfCarry.Text))
                {
                    txtCostOfCarry.Text = "";
                }
                if (!string.IsNullOrEmpty(txtDiscreteEuropean.ToString()))
                {
                    txtDiscreteEuropean.Content = "";
                }
            }
        }

        private void rbEuropean_Checked(object sender, RoutedEventArgs e)
        {
            groupBoxEuropean.IsEnabled = true;
            groupBoxAsian.IsEnabled = false;
            if (groupBoxAsian.IsEnabled == false)
            {
                txtAssetPriceA.Text = "";
                txtRiskFreeRateA.Text = "";
                txtVolatilityA.Text = "";
                startDateA.SelectedDate = null;
                startDateA.Text = "";
                endDateA.SelectedDate = null;
                endDateA.Text = "";
                valuationA.SelectedDate = null;
                valuationA.Text = "";
                if (!string.IsNullOrEmpty(txtAverageSoFarA.Text))
                {
                    txtAverageSoFarA.Text = "";
                }
                if (!string.IsNullOrEmpty(txtCostOfCarryA.Text))
                {
                    txtCostOfCarryA.Text = "";
                }
                if (!string.IsNullOrEmpty(txtDiscreteAsian.ToString()))
                {
                    txtDiscreteAsian.Content = "";
                }
                if (!string.IsNullOrEmpty(txtAsianCurran.ToString()))
                {
                    txtAsianCurran.Content = "";
                }
            }
        }

        private void btnCalculateEuropean_Click(object sender, RoutedEventArgs e)
        {
            Option option;
            OptionCalculator optionCalc = new OptionCalculator();
            var assetPrice = double.Parse(txtAssetPrice.Text);
            var riskFreeRate = double.Parse(txtRiskFreeRate.Text);
            var volatility = double.Parse(txtVolatility.Text);
            double costOfCarry = 0;
            var endPricingDT = DateTime.Parse(endDate.Text);
            var valuationDT = DateTime.Parse(valuationDate.Text);
            double result = 0;
            if (!string.IsNullOrEmpty(txtCostOfCarryA.Text))
            {
                costOfCarry = double.Parse(txtCostOfCarryA.Text);
            }
            if (rbCall.IsChecked==true)
            {
                option = new Option(optionStyle: Option.OptionStyle.European, callPutType: Option.CallPutType.Call,  fixedPrice: double.Parse(txtStrikePrice.Text),commodity:"",fondsCode:"", endPricingPeriod:endPricingDT);
                var optionInput = new EuropeanOptionInput(assetPrice, endPricingDT, valuationDT, riskFreeRate, volatility,
                    costOfCarry);
                result = optionCalc.DiscreteEuropeanHHM(option, optionInput);
            }
            else if (rbPut.IsChecked==true)
            {
                option = new Option(optionStyle: Option.OptionStyle.European, callPutType: Option.CallPutType.Put, fixedPrice: double.Parse(txtStrikePrice.Text), commodity: "", fondsCode: "", endPricingPeriod: endPricingDT);
                var optionInput = new EuropeanOptionInput(assetPrice, endPricingDT, valuationDT, riskFreeRate, volatility,
                    costOfCarry);
                result = optionCalc.DiscreteEuropeanHHM(option, optionInput);
            }
            ShowErrorMessage();
            txtDiscreteEuropean.Content = RoundUpDouble(result);
        }

        private void btnCalculateAsian_Click(object sender, RoutedEventArgs e)
        {
            Option option;
            OptionCalculator optionCalc = new OptionCalculator();
            var assetPrice = double.Parse(txtAssetPriceA.Text);
            var riskFreeRate = double.Parse(txtRiskFreeRateA.Text);
            var volatility = double.Parse(txtVolatilityA.Text);
            var startPricingDT = DateTime.Parse(startDateA.Text);
            var endPricingDT = DateTime.Parse(endDateA.Text);
            var valuationDT = DateTime.Parse(valuationA.Text);
            double averageSoFar = 0;
            double costOfCarry = 0;
            double result1 = 0;
            double result2 = 0;
            if (!string.IsNullOrEmpty(txtAverageSoFarA.Text))
            {
                 averageSoFar = double.Parse(txtAverageSoFarA.Text);
            }
            if (!string.IsNullOrEmpty(txtCostOfCarryA.Text))
            {
                 costOfCarry = double.Parse(txtCostOfCarryA.Text);
            }
            if (rbCall.IsChecked==true)
            {
                option = new Option(optionStyle: Option.OptionStyle.Asian, callPutType: Option.CallPutType.Call, fixedPrice: double.Parse(txtStrikePrice.Text), commodity: "", fondsCode: "", endPricingPeriod: endPricingDT);
                var optionInput = new AsianOptionInput(assetPrice, startPricingDT, endPricingDT, valuationDT, riskFreeRate, volatility,
                    costOfCarry, averageSoFar);
                result2 = optionCalc.AsianCurranApprox(option, optionInput);
                result1 = optionCalc.DiscreteAsianHHM(option, optionInput);
            }
            else if (rbPut.IsChecked==true)
            {
                option = new Option(optionStyle: Option.OptionStyle.Asian, callPutType: Option.CallPutType.Put, fixedPrice: double.Parse(txtStrikePrice.Text), commodity: "", fondsCode: "", endPricingPeriod: endPricingDT);
                var optionInput = new AsianOptionInput(assetPrice, startPricingDT, endPricingDT, valuationDT, riskFreeRate, volatility,
                    costOfCarry, averageSoFar);
                result1 = optionCalc.DiscreteAsianHHM(option, optionInput);
                result2 = optionCalc.AsianCurranApprox(option, optionInput);
            }
            ShowErrorMessage();
            txtDiscreteAsian.Content = RoundUpDouble(result1);
            txtAsianCurran.Content = RoundUpDouble(result2);
        }
        void ShowErrorMessage()
        {
            if (rbCall.IsChecked == false && rbPut.IsChecked == false)
            {
                MessageBox.Show("Please provide a call put type!", "Error");
            }
        }

        string RoundUpDouble(double d)
        {
            var result = Math.Round(d, 4);
            if (result < d)
                result += 0.0001; 
            return result.ToString();
        }
    }
}
