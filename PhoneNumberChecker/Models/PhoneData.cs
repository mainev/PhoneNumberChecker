using PhoneNumbers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sitel.Applications.PhoneNumberChecker.Models
{
    public class PhoneData
    {


        public string NationalFormat
        {
            get
            {
                if (IsNumberValid())
                {
                    return PhoneNumber.NationalNumber.ToString();
                }
                return null;

            }
        }


        public string E164Format
        {
            get
            {
                if (IsNumberValid())
                {
                    return PhoneNumberUtil.Format(PhoneNumber, PhoneNumbers.PhoneNumberFormat.E164);
                }
                return null;

            }
        }

        public string RFC3966Format
        {
            get
            {
                if (IsNumberValid())
                {
                    return PhoneNumberUtil.Format(PhoneNumber, PhoneNumbers.PhoneNumberFormat.RFC3966);
                }
                return null;
            }
        }

       // public bool IsValid { get; set; }

        public  string OriginalPhone { get; set; }
        private readonly int CountryCode;
        private readonly string RegionCode;
        private readonly PhoneNumberUtil PhoneNumberUtil;
        private readonly PhoneNumber PhoneNumber;
        public int Index { get; set; }
        public string[] RawHeaders { get; set; }

        public PhoneData(int countryCode, string phoneNumber, int index)
        {
            Index = index;
            OriginalPhone = phoneNumber;
            CountryCode = countryCode;
            PhoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            RegionCode = PhoneNumberUtil.GetRegionCodeForCountryCode(countryCode);
            PhoneNumber = PhoneNumberUtil.Parse(phoneNumber, RegionCode);
           
        }

        public string RawInput() => PhoneNumber.RawInput;
        public bool IsNumberValid() => PhoneNumberUtil.IsValidNumberForRegion(PhoneNumber, RegionCode);
        public string PhoneNumberType
        {
            get
            {
                try
                {
                    return PhoneNumberUtil.GetNumberType(PhoneNumber).ToString();
                }
                catch (Exception ex)
                {

                    Log.Error(ex, $"Error to get phone number type for phone {PhoneNumber.RawInput}");
                }

                return null;
            }
        }




    }
}
