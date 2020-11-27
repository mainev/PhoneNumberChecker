using System.Collections.Generic;
using System.Linq;

namespace Sitel.Applications.PhoneNumberChecker.Models
{
    public class PhoneDetailsExporter
    {
        public string[] RowArray { get; set; }
        public List<PhoneData> PhoneDatas { get; set; }

        public PhoneDetailsExporter() {
            RowArray = new string[] { };
            PhoneDatas = new List<PhoneData>();
        }

        public string[] EncloseRowArray => RowArray.Select(a => $"\"{a}\"").ToArray();
    }
}
