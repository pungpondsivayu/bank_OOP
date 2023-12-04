using bank_OOP.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace bank_OOP.Class
{
    internal class Data_Management : IData_Management
    {
        public bank Banks { get; set; }
        public List<bank> banks { get; set; }
        public List<SubBank> SubBanks { get; set; }
        public List<debtor> debtors { get; set; }
        public Data_Management()
        {
            Banks = new bank();
            banks = new List<bank>();
            SubBanks = new List<SubBank>();
            debtors = new List<debtor>();
        }

        public void generate()
        {
			GenerateBank();
			generateSubBank();
            Generatedebtor();
        }
        public List<bank> GenerateBank()
        {
            Random r = new Random();
            banks.Add(new bank
            {
                Bank_Id = r.Next(1, 4),
                Bank_Name = "KTM",
                Bank_type = "BankOFThailand",
                Country = "Thailand",
                Profit = 0
            });
            UpdateBank();
            return banks;
        }

        public void UpdateBank()
        {
            using (StreamWriter w = new StreamWriter(@"C:\Users\thepp\Desktop\bank_OOP\Data\bank.txt"))
            {
                foreach (var temp in banks) w.WriteLine($"{temp.Bank_Id}\n{temp.Bank_Name}\n{temp.Bank_type}\n{temp.Country}\n{Banks.getAccumulate()}\n{Banks.Profit}");
                w.Close();
            }
        }
        public List<SubBank> generateSubBank()
        {
            Random r = new Random();
            SubBanks.Add(new SubBank
			{
                Id_SubBank = r.Next(1, 4),
                District = "Kanchanaburi",
                Province = "Kanchanaburi",
            });
			UpdatesubBank();
            return SubBanks;
        }

        public void UpdatesubBank()
        {
            double rate = Banks.Setinterest_rate(0.05);
            using (StreamWriter w = new StreamWriter(@"C:\Users\thepp\Desktop\bank_OOP\Data\SubBank.txt"))
            {
                foreach (var temp in SubBanks) w.WriteLine($"{temp.Id_SubBank}\n{temp.District}\n{temp.Province}\n{rate}\n");
                w.Close();
            }
        }

        public List<debtor> Generatedebtor()
        {
            Random r = new Random();
            for (int i = 0; i < 15; i++)
            {
                debtors.Add(new debtor
				{
                    debtor_Id = i+1,
                    debtor_Name = "Smiley-" + i,
                    Bbf = r.Next(10000, 100000) + r.NextDouble(),
                    Payment_amount = r.Next(1000, 10000) + r.NextDouble(),
					apa = r.Next(0, 80000) + r.NextDouble(),
                });
            }
            Updatedebtor();
            return debtors;
        }
        public void Updatedebtor()
        {
            double rate = Banks.Setinterest_rate(0.05);
            using (StreamWriter w = new StreamWriter(@"C:\Users\thepp\Desktop\bank_OOP\Data\debtor.txt"))
            {
                foreach (var temp in debtors) w.WriteLine($"{temp.debtor_Id} {temp.debtor_Name} {temp.Bbf} {temp.Payment_amount} {temp.apa}");
                w.Close();
            }
        }
    }
}

