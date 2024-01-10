using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailySiteCheckup.Feature
{
    public class ReadFromExcel
    {
        // Create a dictionary to store the grouped data
        Dictionary<string, List<string>> groupedData { get; set; }
        
       public static Dictionary<string, Dictionary<string, string>> SiteDetailsDic { get; set; }
       public static Dictionary<string, Dictionary<string, string>> SignupDetailsDic { get; set; }
        public static Dictionary<string, string>[] dictionaries = new Dictionary<string, string>[2];
        public static string SiteURL { get; set; }
        public static string SiteName { get; set; }
        public static string IsLoginPageCookiePopup { get; set; }
        public static string IsSignupPageCookiePopup { get; set; }
        public static string IsConsentPopup { get; set; }
        public static string ConsentPopupChkBx1 { get; set; }
        public static string ConsentPopupChkBx2 { get; set; }
        // public static Dictionary<string, Dictionary<string, string>>[] dictionaries { get; set; }

        public ReadFromExcel()
        {
            groupedData = new Dictionary<string, List<string>>();
            SiteDetailsDic = new Dictionary<string, Dictionary<string, string>>();
            SignupDetailsDic = new Dictionary<string, Dictionary<string, string>>();
           
            // ReturnSiteData();
        }
        public static void ReturnSiteData()
        {
            SiteDetailsDic = GroupSiteandColumns("C:\\Users\\Merlin.Savarimuthu\\Reports\\EmailSetting\\SiteCheck_TestData.xlsx", 0);
            SignupDetailsDic = GroupSiteandColumns("C:\\Users\\Merlin.Savarimuthu\\Reports\\EmailSetting\\SiteCheck_TestData.xlsx", 1);
            //dictionaries[0] = SiteDetailsDic;
            //dictionaries[1] = SignupDetailsDic;
            //return dictionaries;
        }
        public static Dictionary<string,Dictionary<string,string>> GroupSiteandColumns(string filePath,int worksheet_num)
        {
            Dictionary<string, Dictionary<string, string>> nestedDictionary = new Dictionary<string, Dictionary<string, string>>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                // Load the Excel file
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    // Get the first worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheet_num];

                    // Initialize a nested dictionary to store data
                    int headerRow = 1;

                    // Start from row 2 to read data
                    for (int row = headerRow + 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        // Initialize a dictionary to store values for the current row
                        Dictionary<string, string> rowData = new Dictionary<string, string>();

                        // Iterate through the columns
                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            // Get the header text from the header row
                            string header = worksheet.Cells[headerRow, col].Text;

                            // Get the value from the current row and column
                            string value = worksheet.Cells[row, col].Text;

                            // Add the value to the current row's dictionary
                            rowData.Add(header, value);
                        }

                        // Get the key for the nested dictionary
                        string key = worksheet.Cells[row, 1].Text; 

                        // Add the current row's dictionary to the nested dictionary
                        nestedDictionary.Add(key, rowData);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return nestedDictionary;

        }
    }
}
