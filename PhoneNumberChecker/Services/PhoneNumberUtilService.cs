using PhoneNumbers;
using Sitel.Applications.PhoneNumberChecker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sitel.Applications.PhoneNumberChecker.Services
{
    public static class PhoneNumberUtilService
    {
        public static List<string> GetSupportedRegionCodes()
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var regions = new List<string>();

            foreach (var reg in phoneNumberUtil.GetSupportedRegions())
                regions.Add(reg);

            return regions;
        }

        public static List<PhoneCode> GetPhoneCodes()
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var codes = new List<PhoneCode>();

            foreach (var rg in phoneNumberUtil.GetSupportedRegions())
            {
                var countryCode = phoneNumberUtil.GetCountryCodeForRegion(rg);
                codes.Add(new PhoneCode
                {
                    RegionCode = rg,
                    CountryCode = countryCode
                });

            }

            return codes;
        }
    }
}
