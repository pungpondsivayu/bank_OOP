using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_OOP.Class
{
    public class bank : SubBank
    {
        public int Bank_Id { get; set; }
        public string Bank_Name { get; set; }
        public string Bank_type { get; set; }
        public string Country { get; set; }
        private double Accumulated_balance { get; set; }
        internal double Profit { get; set; }

        public double updateProfit(double value = 0) => Profit = value; 
        public double SetAccumulate(double value) => Accumulated_balance = value;
        public double getAccumulate() => Accumulated_balance;
    }
}
