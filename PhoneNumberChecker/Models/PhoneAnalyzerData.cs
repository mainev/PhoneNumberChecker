using System;
using System.Collections.Generic;
using System.Text;

namespace Sitel.Applications.PhoneNumberChecker.Models
{
    public class PhoneAnalyzerData
    {
        public string[] RowArray { get; set; }
        public List<PhoneData> PhoneDatas { get; set; }

        public PhoneAnalyzerData() {
            RowArray = new string[] { };
            PhoneDatas = new List<PhoneData>();
        }
    }
}
