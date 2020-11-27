using PhoneNumbers;
using Serilog;
using Sitel.Applications.PhoneNumberChecker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TextFieldParserCore;

namespace Sitel.Applications.PhoneNumberChecker
{
    public class PhoneAnalyzer
    {

        public string File { get; set; }
        public List<int> PhoneIndex { get; set; }
        public bool ProvideNationalNumber { get; set; }
        public bool ProvideE164Number { get; set; }
        public bool ProvideRFC3955Number { get; set; }
        public bool ExcludeFirstRow { get; set; }
        public string Delimiter { get; set; }

        public List<PhoneDetailsExporter> PhoneAnalyzerDatas { get; set; }

        private string[] RawHeaders { get; set; }
        public int CountryCode { get; set; }
        public string ExportDirectory { get; set; }

        public PhoneAnalyzer()
        {
            PhoneIndex = new List<int>();
            PhoneAnalyzerDatas = new List<PhoneDetailsExporter>();
        }

        public void RunAnalyzer()
        {
            using var reader = new StreamReader(File);
            int row = 1;

            Log.Information($"Scanning file {File}");
            while (!reader.EndOfStream)
            {

                var phoneAnalyzerData = new PhoneDetailsExporter();

                string rawLine = reader.ReadLine();

                if (RawHeaders == null)
                    RawHeaders = rawLine.Split(Delimiter);

                if (ExcludeFirstRow && row == 1)
                {
                    row++;
                    continue;
                }

                try
                {

                    if (String.IsNullOrEmpty(rawLine))
                    { continue; }

                    TextFieldParser parser = new TextFieldParser(new StringReader(rawLine))
                    {
                        HasFieldsEnclosedInQuotes = true
                    };

                    parser.SetDelimiters(Delimiter);
                    string[] lineArray = parser.ReadFields();

                    phoneAnalyzerData.RowArray = lineArray;

                    for (int i = 0; i < lineArray.Length; i++)
                    {
                        if (PhoneIndex.Contains(i))
                        {
                            var phoneData = new PhoneData(CountryCode, lineArray[i], i);
                            
                            phoneAnalyzerData.PhoneDatas.Add(phoneData);

                            Log.Information($"Row {row} = Phone: {phoneData.OriginalPhone} Type: {phoneData.PhoneNumberType}");
                        }
                    }


                    PhoneAnalyzerDatas.Add(phoneAnalyzerData);

                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"An error occurred on row: {row}, lineData:[{rawLine}]");
                }

                row++;
            }
        }



        public void GenerateFile(string filename)
        {
            Log.Information($"Export directory: {ExportDirectory}");

            if (!Directory.Exists(ExportDirectory))
            {
                Directory.CreateDirectory(ExportDirectory);
            }

            filename = Path.Combine(ExportDirectory, filename);

            try
            {
                using StreamWriter outputFile = new StreamWriter(filename, true);
                outputFile.WriteLine(GetOutputHeader());
                string rawFile = new FileInfo(File).Name;

                foreach (var pData in PhoneAnalyzerDatas)
                {
                    var phonesStat = pData.PhoneDatas.Select(p => ConvertPhoneDataToString(p)).ToList();
                    var phoneCheckerRow = $"{String.Join(',', pData.EncloseRowArray)},{String.Join(',', phonesStat)},{rawFile}";
                    outputFile.WriteLine(phoneCheckerRow);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error to create export file {filename}.");
            }

            if (System.IO.File.Exists(filename))
                Log.Information($"Export file {filename} successfully generated.");
            else
                Log.Error($"Error to generate {filename}");
            
        }

        public string GetOutputHeader()
        {

            var headerList = new List<string>();
            if (ExcludeFirstRow)
                headerList.AddRange(RawHeaders.ToList());

            else
                headerList.AddRange(RawHeaders.Select((item, index) => $"COLUMN{index}"));

            for (int index = 1; index <= PhoneIndex.Count; index++)
            {
                headerList.Add($"PHONE{index}_TYPE");

                if (ProvideNationalNumber)
                {
                    headerList.Add($"PHONE{index}_NATIONAL");
                }


                if (ProvideE164Number)
                {
                    headerList.Add($"PHONE{index}_E164");
                }

                if (ProvideRFC3955Number)
                {
                    headerList.Add($"PHONE{index}_RFC3955");
                }
            }


            headerList.Add($"RAWFILE");
            return String.Join(',', headerList);
        }

        public string ConvertPhoneDataToString(PhoneData phoneData)
        {
            var resultList = new List<string>();

            resultList.Add(phoneData.PhoneNumberType ?? "INVALID");

            if (ProvideNationalNumber)
            {
                resultList.Add(phoneData.NationalFormat);
            }


            if (ProvideE164Number)
            {
                resultList.Add(phoneData.E164Format);
            }

            if (ProvideRFC3955Number)
            {
                resultList.Add(phoneData.RFC3966Format);
            }

       
            return String.Join(',', resultList);
        }
    }


}
