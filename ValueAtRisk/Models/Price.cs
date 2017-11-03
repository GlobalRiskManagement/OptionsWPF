using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
    public class Price
    {
        /// <summary>
        /// The default constructor of the class
        /// </summary>
        /// <param name="price">The value of the price</param>
        /// <param name="date">The date when the price was collected. Not to be confused with the forward date to which the price refers</param>
        public Price(double price, DateTime date)
        {
            if (price <= 0)
            {
                //todo: take care of the logger line below
                //Logger.Log(Logger.Level.Severe, "The price provided is negative or equal to 0 and prices cannot be negative or 0: " + price);
                throw new Exception("The price provided is negative or equal to 0 and prices cannot be negative or 0: " + price);
            }
            Value = price;
            Date = date;
        }
        /// <summary>
        /// The numeric value of the price
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// The date when the price was collected. Not to be confused with the forward date to which the price refers
        /// </summary>
        public DateTime Date { get; set; }
    }
}
