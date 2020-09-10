using PhoneNumbers;
using Serilog;
using Serilog.Events;
using Sitel.Applications.PhoneNumberChecker.Exceptions;
using System;
using System.Globalization;
using System.Linq;

namespace Sitel.Applications.PhoneNumberChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
          
         
            //Usage:
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("+44123456789", "GB"), "GB"));
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("0756325489", "GB"), "GB"));
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("07563254892", "GB"), "GB"));
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("07198856320", "GB"), "GB"));
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("+447198856320", "GB"), "GB"));
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("+44-7198856320", "GB"), "GB"));
            //Console.WriteLine(phoneNumberUtil.IsValidNumberForRegion(phoneNumberUtil.Parse("0044-7198856320", "GB"), "GB"));
            //var validPhone = phoneNumberUtil.Parse("0044-7198856320", "GB");
            //var nationalNumber = validPhone.NationalNumber;
            //var internationalNumber = phoneNumberUtil.Format(validPhone, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
            //var e164Number = phoneNumberUtil.Format(validPhone, PhoneNumbers.PhoneNumberFormat.E164);
            //var rfc3966Number = phoneNumberUtil.Format(validPhone, PhoneNumbers.PhoneNumberFormat.RFC3966);
            //var nationalNumber2 = phoneNumberUtil.Format(validPhone, PhoneNumbers.PhoneNumberFormat.NATIONAL);
            //var numberType = phoneNumberUtil.GetNumberType(validPhone);
            //var regionCode = phoneNumberUtil.GetRegionCodeForCountryCode(44);


            //algorithm
            //read file, analyze phone numbers, create output file with numberType, phone is valid, filename, 
            //options: countryCode=44 file="FILE.csv" phoneIndex=1|2|3 -n (option to output national number output)
            string name = args[0];
            string TASK_ID = $"{DateTime.Now:yyyyMMddHHmmss}";
            Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                     .Enrich.FromLogContext()
                     .WriteTo.File($"./Logs/{name}_{TASK_ID}.log")
                     .CreateLogger();

            Log.Information("Process STARTED");
            Log.Information($"ID: {TASK_ID}");
            

            AnalyzeCommandLineArgs(args, out PhoneAnalyzer phoneAnalyzer);

            phoneAnalyzer.RunAnalyzer();
            phoneAnalyzer.GenerateFile($"PhoneNumberChecker_{TASK_ID}.csv");

            Log.Information("Process ENDED.");

        }


        public static void AnalyzeCommandLineArgs(string[] args, out PhoneAnalyzer phoneAnalyzer) 
        {
            phoneAnalyzer = new PhoneAnalyzer();
            for (int i = 0; i < args.Count(); i++)
            {
                if (i > 0)
                {
                    var arg = args[i];

                    string optionKey = "";
                    string optionValue = "";

                    if (arg.Substring(0, 1) == "-")
                    {
                        optionKey = arg;
                    }
                    else if (arg.Contains("="))
                    {
                        string[] valuesArray = arg.Split("=");
                        optionKey = valuesArray[0];
                        optionValue = valuesArray[1];
                    }
                    else
                    {
                        throw new InvalidCommandLineArgumentsException(arg);
                    }

                    switch (optionKey)
                    {
                        case "phoneIndex":
                            phoneAnalyzer.PhoneIndex.AddRange(optionValue.Split("|").Select(o => Convert.ToInt32(o)));
                            break;
                        case "countryCode":
                            phoneAnalyzer.CountryCode = Convert.ToInt32(optionValue);
                            break;
                        case "file":
                            phoneAnalyzer.File = optionValue;
                            break;
                        case "delimiter":
                            phoneAnalyzer.Delimiter = optionValue;
                            break;
                        case "exportDirectory":
                            phoneAnalyzer.ExportDirectory = optionValue;
                            break;
                        case "-national":
                            phoneAnalyzer.ProvideNationalNumber = true;
                            break;
                        case "-e164":
                            phoneAnalyzer.ProvideE164Number = true;
                            break;
                        case "-rfc3966":
                            phoneAnalyzer.ProvideRFC3955Number = true;
                            break;
                        case "-h":
                            phoneAnalyzer.ExcludeFirstRow = true;
                            break;
                        default:
                            throw new InvalidCommandLineArgumentsException(optionKey);

                    }
                }

            }
        }
    }
}
