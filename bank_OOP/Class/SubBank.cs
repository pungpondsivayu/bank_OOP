using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_OOP.Class
{
    public class SubBank : debtor
    {
        public int Id_SubBank { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        protected double interest_rate { get; set; }
        public double Setinterest_rate(double value)
        {
            return interest_rate = value;
        }

        public double Getinterest_rate()
        {
            return interest_rate;
        }

	}
}
