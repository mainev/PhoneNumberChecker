using System;
using System.Collections.Generic;
using System.Text;

namespace Sitel.Applications.PhoneNumberChecker.Exceptions
{
    public class PhoneNumberInvalidException : Exception
    {
        public PhoneNumberInvalidException(string phone, string region) : base($"String provided \"{phone}\" is not a valid phone number for region \"{region}\".") { }
    }
}
