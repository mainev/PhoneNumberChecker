using PhoneNumbers;
using Serilog;
using Sitel.Applications.PhoneNumberChecker.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sitel.Applications.PhoneNumberChecker.Models
{
    public class PhoneData
    {
        
        public string InternationalFormat
        {
            get
            {

                if (IsNumberValid())
                {
                    return PhoneNumberUtil.Format(PhoneNumber, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
                }
                else if (IgnoreInvalid)
                {
                    return OriginalPhone;
                }

                throw new PhoneNumberInvalidException(OriginalPhone, RegionCode);

            }
        }

        public string NationalFormat
        {
            get
            {
                if (IsNumberValid())
                {
                    return PhoneNumber.NationalNumber.ToString();
                }
                else if (IgnoreInvalid)
                {
                    return OriginalPhone;
                }

                throw new PhoneNumberInvalidException(OriginalPhone, RegionCode);
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
                else if (IgnoreInvalid)
                {
                    return OriginalPhone;
                }

                throw new PhoneNumberInvalidException(OriginalPhone, RegionCode);

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
                else if (IgnoreInvalid) {
                    return OriginalPhone;
                }

                throw new PhoneNumberInvalidException(OriginalPhone, RegionCode);
            }
        }


        public  string OriginalPhone { get; set; }
        private readonly int CountryCode;
        private readonly string RegionCode;
        private readonly PhoneNumberUtil PhoneNumberUtil;
        private readonly PhoneNumber PhoneNumber;
        public int Index { get; set; }
        private bool IgnoreInvalid = false;
        public int GetCountryCode() => CountryCode;
        /// <summary>
        /// Creates instance of phone data with country code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="ignoreInvalid"></param>
        public PhoneData(int countryCode, string phoneNumber, bool ignoreInvalid = false)
        {
            OriginalPhone = phoneNumber;
            CountryCode = countryCode;
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
            RegionCode = PhoneNumberUtil.GetRegionCodeForCountryCode(countryCode);
            PhoneNumber = String.IsNullOrEmpty(phoneNumber) ? null : PhoneNumberUtil.Parse(phoneNumber, RegionCode);
            IgnoreInvalid = ignoreInvalid;
        }

        /// <summary>
        /// Creates instance of phone data with country code and file index.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="index"></param>
        public PhoneData(int countryCode, string phoneNumber, int index)
        {
            Index = index;
            OriginalPhone = phoneNumber;
            CountryCode = countryCode;
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
            RegionCode = PhoneNumberUtil.GetRegionCodeForCountryCode(countryCode);
            PhoneNumber = String.IsNullOrEmpty(phoneNumber) ? null : PhoneNumberUtil.Parse(phoneNumber, RegionCode);
        }

        /// <summary>
        /// Create instance of phone data without index.
        /// </summary>
        /// <param name="regionCode">Supported region codes</param>
        /// <param name="phoneNumber">Phone number to format</param>
        /// <param name="ignoreInvalid">If true, returns original format.</param>
        public PhoneData(string regionCode, string phoneNumber, bool ignoreInvalid = false)
        {
            OriginalPhone = phoneNumber;
            RegionCode = regionCode;
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
            CountryCode = PhoneNumberUtil.GetCountryCodeForRegion(regionCode);
            PhoneNumber = String.IsNullOrEmpty(phoneNumber) ? null : PhoneNumberUtil.Parse(phoneNumber, RegionCode);
            IgnoreInvalid = ignoreInvalid;
        }

        /// <summary>
        /// Creates instance of phone data with index.
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="index">Index of phone in the file.</param>
        public PhoneData(string regionCode, string phoneNumber, int index)
        {
            Index = index;
            OriginalPhone = phoneNumber;
            RegionCode = regionCode;
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
            CountryCode = PhoneNumberUtil.GetCountryCodeForRegion(regionCode);
            PhoneNumber = String.IsNullOrEmpty(phoneNumber) ? null : PhoneNumberUtil.Parse(phoneNumber, RegionCode);

        }

        public string RawInput() => PhoneNumber.RawInput;

        public bool IsNumberValid() => PhoneNumber != null && PhoneNumberUtil.IsValidNumberForRegion(PhoneNumber, RegionCode);

        public string PhoneNumberType
        {
            get
            {
                if (PhoneNumber == null) return PhoneNumbers.PhoneNumberType.UNKNOWN.ToString();

                return PhoneNumberUtil.GetNumberType(PhoneNumber).ToString();
                //try
                //{
                //    return PhoneNumberUtil.GetNumberType(PhoneNumber).ToString();
                //}
                //catch (Exception ex)
                //{

                //    Log.Error(ex, $"Error to get phone number type for phone {PhoneNumber.RawInput}");
                //}

                //return null;
            }
        }

        public string AttemptDetectRegionCode() {
            return PhoneNumberUtil.GetRegionCodeForCountryCode(PhoneNumber.CountryCode);
        }

        
        
     




    }
}
