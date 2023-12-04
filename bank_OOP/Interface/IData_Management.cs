using bank_OOP.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_OOP.Interface
{
	public interface IData_Management
	{
		void generate();
		List<bank> GenerateBank();
		List<SubBank> generateSubBank();
		List<debtor> Generatedebtor();
		void UpdateBank();
		void UpdatesubBank();
		void Updatedebtor();
	}
}
