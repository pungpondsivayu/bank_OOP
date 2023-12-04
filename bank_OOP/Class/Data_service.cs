using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace bank_OOP.Class
{
    internal class Data_service
    {
        public Data_Management Data_Managements { get; set; }
        public List<debtor> debtors { get; set; }
        public double Rate;
        public Data_service()
        {
            debtors = new List<debtor>();
            Data_Managements = new Data_Management();
            Data_Managements.generate(); // เพื่อสร้างข้อมูลจำลอง
			Rate = Data_Managements.Banks.Getinterest_rate();  //เพื่อนำข้อมูล อัตตราดอกเบี้ย ที่สร้างขึ้น มาเก็บไว้ในตัวแปร Rate
			loadDebtor();
			updateBank();
            load_header();
			// 1. โหลด ข้อมูลลูกหนี้
			// 2. นำค่าของลูกหนี้มาคำนวนแล้วเรียกใช้ฟังชั้น updateBank() เพื่ออัปเดทข้อมูลใน textfile
            // 3. โหลดข้อมูลส่วนของหัวตาราง โดยประกอบด้วย textfile 2 file คือ bank.txt และ SubBank.txt
		}

		void updateBank() // หลังจากโหลดข้อมูลจาก textfile ลูกหนี้แล้ว จากนั้นนำไปคำนวนแล้วอัปเดท textfile Bank
        {
            var Accumulate = debtors.Sum(e => e.Bbf);
            var profit = debtors.Sum(e => e.apa) - debtors.Sum(e => e.Payment_amount);
			Data_Managements.Banks.SetAccumulate(Accumulate);
			Data_Managements.Banks.updateProfit(profit);
			Data_Managements.UpdateBank();
        }
        public List<debtor> loadDebtor() //อ่านค่าข้อมูลของลูกหนี้จาก Textfile แล้วนำไปเก็บไว้ใน List<debtor>
        {
            string[] file = File.ReadAllLines(@"C:\Users\thepp\Desktop\bank_OOP\Data\debtor.txt");
			foreach (var item in file)
            {
                string[] data = item.Split(' ');
                debtors.Add(new debtor
                {
					debtor_Id = Convert.ToInt32(data[0]),
					debtor_Name = data[1],
					Bbf = Convert.ToDouble(data[2]),
					Payment_amount = Convert.ToDouble(data[3]),
					apa = Convert.ToDouble(data[4]),
				});
            }
            return debtors;
		}
        
        public List<string> load_header() // อ่านค่าจากtextfile ของ bank,sunbank แล้วนำมาต่อกันโดยเก็บไว้ใน List ชื่อ newData
        {
			string[] filePathBank = File.ReadAllLines(@"C:\Users\thepp\Desktop\bank_OOP\Data\bank.txt");
			string[] filePathSubbank = File.ReadAllLines(@"C:\Users\thepp\Desktop\bank_OOP\Data\Subbank.txt");
            var newData = new List<string>(filePathBank.Concat(filePathSubbank));
            return newData;
        }
        public void Display_data() // แสดงผลและคำนวน ลูกหนี้
        {
            var data_temps = new List<data_temp>();
            double SDirate, SDbcf;
            var Dataheader = load_header();
			// เลขอินเดก ใน Array รหัวตาราง
			// 0.รหัสธนาคาร 
			// 1.ชื่อธนาคาร 
			// 2.ประเภทธนาคาร
			// 3.ประเทศ
			// 4.ยอดเงินสะสม 
			// 5.กำไร 
			// 6.รหัสสาขาย่อย 
			// 7.อำเภอ 
			// 8.จังหวัด 
			// 9.อัตตราดอกเบี้ย 
			Console.WriteLine("=======================================================================================");
			Console.WriteLine($"Branch sequence : {Dataheader[0]} Bank name : {Dataheader[1]} Accumulated balance : {Convert.ToDouble(Dataheader[4]).ToString("#.##")} " +
				$"Profit : {Convert.ToDouble(Dataheader[5]).ToString("#.##")}|\n\ncountry : {Dataheader[3]} District : {Dataheader[7]} Province : {Dataheader[8]}{"",20}|");
			Console.WriteLine("=======================================================================================");
            Console.WriteLine($"   ID   |{"Name",8}{"",4}|{"Bbf",8}{"",4}|{"Payment_A",10}{"",2}|" +
                $"{"Irate",8}{"",4}|{"Apa",8}{"",4}|{"Bcf",8}{"",4}|");
            Console.WriteLine("=======================================================================================");
            foreach (var item in debtors) // แสดงผล ข้อมูลลูกหนี้ 
            {
                double rate = Irate(item.Bbf, item.Payment_amount);
                double bcf = Bcf(item.Bbf, item.Payment_amount, rate, item.apa);
                Console.WriteLine($"   {item.debtor_Id,2}   |{item.debtor_Name,10}{"",2}|" +
                    $"{item.Bbf.ToString("#.##"),10}{"",2}|{item.Payment_amount.ToString("#.##"),10}{"",2}" +
                    $"|{rate.ToString("#.##"),10}{"",2}|{item.apa.ToString("#.##"),10}{"",2}|" +
                    $"{bcf.ToString("#.##"),10}{"",2}|");
                data_temps.Add(new data_temp
                {
                    Irate_temp = rate,
                    bcf_temp = bcf
                });
            }
			SDirate = SDIrate_cal(data_temps);
            SDbcf = SDbcf_cal(data_temps);
			Console.WriteLine("=======================================================================================");
            Console.WriteLine($"{"",37}Sum{"",7}|{data_temps.Sum(e => e.Irate_temp).ToString("#.##"),10}" + // แสดงผล ผลรวม ดอกเบี้ย
							  $"{"",2}|{debtors.Sum(e => e.apa).ToString("#.##"),10}{"",2}|" + // แสดงผล ผลรวม ยอดซื้อเพิ่ม
							  $"{data_temps.Sum(e => e.bcf_temp).ToString("#.##"),10}{"",2}|"); // แสดงผล ผลรวม ยอดคงเหลือยกไป

			Console.WriteLine($"{"",37}Average{"",3}|{data_temps.Average(e => e.Irate_temp).ToString("#.##"),10}{"",2}|" + // แสดงผล ผลรวม ดอกเบี้ย
							  $"{debtors.Average(e => e.apa).ToString("#.##"),10}{"",2}|" + // แสดงผล ผลรวม ยอดซื้อเพิ่ม
							  $"{data_temps.Average(e => e.bcf_temp).ToString("#.##"),10}{"",2}|"); // แสดงผล ผลรวม ยอดคงเหลือยกไป

			Console.WriteLine($"{"",37}SD{"",8}|{SDirate.ToString("#.##"),10}{"",2}|" + // แสดงผล ผลรวม ดอกเบี้ย
							  $"{SDIrate_cal(debtors).ToString("#.##"),10}{"",2}|" + // แสดงผล ผลรวม ยอดซื้อเพิ่ม
							  $"{SDbcf.ToString("#.##"),10}{"",2}|"); // แสดงผล ผลรวม ยอดคงเหลือยกไป
			Console.WriteLine("=======================================================================================");
		}
		public double SDIrate_cal(List<debtor> data) // รับค่า แล้วนำมาคำนวน ส่วนเบียงเบนมาตรฐานดอกเบี้ย
		{
			double mean = data.Average(e => e.apa);
			double sumSquaredDifferences = data.Sum(e => Math.Pow(e.apa - mean, 2));
			double populationVariance = sumSquaredDifferences / data.Count;
			return populationVariance = Math.Sqrt(populationVariance);
		}
		public double SDIrate_cal(List<data_temp> data) // รับค่า แล้วนำมาคำนวน ส่วนเบียงเบนมาตรฐานยอดคงเหลือยกไป
		{
			double mean = data.Average(e => e.bcf_temp);
			double sumSquaredDifferences = data.Sum(e => Math.Pow(e.bcf_temp - mean, 2));
			double populationVariance = sumSquaredDifferences / data.Count;
			return populationVariance = Math.Sqrt(populationVariance);
		}
        public double SDbcf_cal(List<data_temp> data) // รับค่า แล้วนำมาคำนวน ส่วนเบียงเบนมาตรฐานยอกซื้อเพิ่ม
		{
			double mean = data.Average(e => e.bcf_temp);
			double sumSquaredDifferences = data.Sum(e => Math.Pow(e.bcf_temp - mean, 2));
			double populationVariance = sumSquaredDifferences / data.Count;
			return populationVariance = Math.Sqrt(populationVariance);
		}

		public double Irate(double Bbf, double Payment_amount) => (Bbf - Payment_amount) * Rate; 
		public double Bcf(double Bbf, double Payment_amount, double Irate, double apa)	=> Bbf - Payment_amount + Irate + apa;
	}

    public class data_temp
    {
        public double Irate_temp { get; set; }
        public double bcf_temp { get; set; }
    }
  }

