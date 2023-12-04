using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_OOP.Class
{
    public class debtor
    {
        public int debtor_Id { get; set; }
        public string debtor_Name { get; set; }
        public double Bbf { get; set; } //Balance brought forward
        public double Payment_amount { get; set; }
        public double apa  { get; set; } //additional purchase amount
    }
}
