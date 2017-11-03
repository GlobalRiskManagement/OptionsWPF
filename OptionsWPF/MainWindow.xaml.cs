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
using ValueAtRisk.Calculations;
using ValueAtRisk.Models;

namespace OptionsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        }

        private void rbEuropean_Checked(object sender, RoutedEventArgs e)
        {
            groupBoxEuropean.IsEnabled = true;
            groupBoxAsian.IsEnabled = false;
        }

        private void btnCalculateEuropean_Click(object sender, RoutedEventArgs e)
        {
            Option option;
            OptionInput optionInput;
            OptionCalculator optionCalc = new OptionCalculator();
            var assetPrice = double.Parse(txtAssetPrice.Text);
            var riskFreeRate = double.Parse(txtRiskFreeRate.Text);
            var volatility = double.Parse(txtVolatility.Text);
            var costOfCarry = double.Parse(txtCostOfCarry.Text);
            var endPricingDT = DateTime.Parse(endDate.Text);
            var valuationDT = DateTime.Parse(valuationDate.Text);
            double result = 0;
            if (rbCall.IsChecked==true)
            {
                option = new Option(callPutType: Option.CallPutType.Call, optionStyle: Option.OptionStyle.European, strikePrice: double.Parse(txtStrikePrice.Text));
                optionInput = new OptionInput(assetPrice, endPricingDT, valuationDT, riskFreeRate, volatility,
                    costOfCarry);
                result = optionCalc.DiscreteEuropeanHHM(option, optionInput);
            }
            else if (rbPut.IsChecked==true)
            {
                option = new Option(callPutType: Option.CallPutType.Put, optionStyle: Option.OptionStyle.European, strikePrice: double.Parse(txtStrikePrice.Text));
                optionInput = new OptionInput(assetPrice, endPricingDT, valuationDT, riskFreeRate, volatility,
                    costOfCarry);
                result = optionCalc.DiscreteEuropeanHHM(option, optionInput);
            }
            ShowErrorMessage();
            txtDiscreteEuropean.Content = result.ToString();
        }

        private void btnCalculateAsian_Click(object sender, RoutedEventArgs e)
        {
            Option option;
            OptionInputAsian optionInput;
            OptionCalculator optionCalc = new OptionCalculator();
            var assetPrice = double.Parse(txtAssetPriceA.Text);
            var riskFreeRate = double.Parse(txtRiskFreeRateA.Text);
            var volatility = double.Parse(txtVolatilityA.Text);
            var costOfCarry = double.Parse(txtCostOfCarryA.Text);
            var startPricingDT = DateTime.Parse(startDateA.Text);
            var endPricingDT = DateTime.Parse(endDateA.Text);
            var valuationDT = DateTime.Parse(valuationA.Text);
            var averageSoFar = double.Parse(txtAverageSoFarA.Text);
            var noOfFixingsFixed = int.Parse(txtNoFixingsFixedA.Text);
            double result1 = 0;
            double result2 = 0;
            if (rbCall.IsChecked==true)
            {
                option = new Option(callPutType: Option.CallPutType.Call, optionStyle: Option.OptionStyle.Asian, strikePrice: double.Parse(txtStrikePrice.Text));
                optionInput = new OptionInputAsian(assetPrice, averageSoFar, startPricingDT, endPricingDT, valuationDT, riskFreeRate, volatility, noOfFixingsFixed,
                    costOfCarry);
                result2 = optionCalc.AsianCurranApprox(option, optionInput);
                result1 = optionCalc.DiscreteAsianHHM(option, optionInput);
            }
            else if (rbPut.IsChecked==true)
            {
                option = new Option(callPutType: Option.CallPutType.Put, optionStyle: Option.OptionStyle.Asian, strikePrice: double.Parse(txtStrikePrice.Text));
                optionInput = new OptionInputAsian(assetPrice, averageSoFar, startPricingDT, endPricingDT, valuationDT, riskFreeRate, volatility, noOfFixingsFixed,
                    costOfCarry);
                result1 = optionCalc.DiscreteAsianHHM(option, optionInput);
                result2 = optionCalc.AsianCurranApprox(option, optionInput);
            }
            ShowErrorMessage();
            txtDiscreteAsian.Content = result1.ToString();
            txtAsianCurran.Content = result2.ToString();
        }
        void ShowErrorMessage()
        {
            if (rbCall.IsChecked == false && rbPut.IsChecked == false)
            {
                MessageBox.Show("Please provide a call put type!", "Error");
            }
        }
    }
}
