using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using NUnitLite;
using System.Reflection;
using OpenQA.Selenium.Interactions;
using System.Xml;
using OfficeOpenXml;
using System.IO;
using DailySiteCheckup.Feature;
using Microsoft.SharePoint.News.DataModel;

namespace DailySiteCheckup.Feature
{
    public class WriteExcelTestResult
    {
        public void SaveToExcel()
        {
            var testResults = TestContext.CurrentContext.Result.Outcome;
            // get today's date
            DateTime currentDate = DateTime.Now;
            string currdate = currentDate.ToShortDateString();
            string date_For_File_Path = currdate.Replace("/", "");

            string resultFilePath = "C:\\Users\\Merlin.Savarimuthu\\Reports\\TestReport_" + date_For_File_Path;
            // Create XML writer settings
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            };

            // Create the XML writer
            using (var writer = XmlWriter.Create(resultFilePath, settings))
            {
                // Start the XML document
                writer.WriteStartDocument();
                writer.WriteStartElement("TestResults");

                // Write the test result
                writer.WriteStartElement("TestResult");
                writer.WriteAttributeString("Outcome", testResults.ToString());
                writer.WriteEndElement();

                // End the XML document
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }

        public void WriteTestResultsToExcel(string filePath, List<TestResult> testResults1)
        {
            DateTime currentDate = DateTime.Now;
            string currdate = currentDate.ToShortDateString();
            string sheetdate = currdate.Replace("/", "");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<TestResult> testResults = new List<TestResult>
                    {
                        new TestResult { SiteURL = "https://jeffersonwells.no", SiteName = "NO_Jefferson wells", SignUpStatus = "Y",LoginStatus=null, Message = "Success", TestCaseName = "Signup" }
                          };
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {

                // Create a new worksheet
                //var worksheet = package.Workbook.Worksheets.Add("TestResults_" + sheetdate);
                var worksheet = package.Workbook.Worksheets.SingleOrDefault(ws => ws.Name.Trim() == "TestResults");
                if (worksheet != null)
                {
                    int lastRow = worksheet.Dimension.End.Row;
                    int lastColumn = worksheet.Dimension.End.Column;
                    for (int row = 1; row <= lastRow; row++)
                    {
                        bool isEmptyRow = true;

                        for (int col = 1; col <= lastColumn; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text;
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                isEmptyRow = false;
                                break;
                            }
                        }

                        if (isEmptyRow)
                        {
                            // Fill data into empty cells of the empty row
                            worksheet.Cells[row, 1].Value = "Value1";
                            worksheet.Cells[row, 2].Value = "Value2";
                            // ... Repeat for other columns
                        }
                    }

                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add("TestResults");
                    worksheet.Cells[1, 1].Value = "Sites";
                    worksheet.Cells[1, 2].Value = "URL";
                    worksheet.Cells[1, 3].Value = "Signup";
                    worksheet.Cells[1, 4].Value = "Login";
                    worksheet.Cells[1, 5].Value = "FP";

                    // Write test results to the worksheet
                    for (int i = 0; i < testResults.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = testResults[i].SiteName;
                        worksheet.Cells[i + 2, 2].Value = testResults[i].SiteURL;
                        worksheet.Cells[i + 2, 3].Value = testResults[i].SignUpStatus;
                        worksheet.Cells[i + 2, 4].Value = testResults[i].LoginStatus == null ? "N" : testResults[i].LoginStatus;
                    }
                }



                package.Save();
                // Save the Excel package to a file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
                string resultFilePath = "C:\\Users\\Merlin.Savarimuthu\\Reports\\EmailSetting\\SiteCheck_TestResult.xlsx";
                WriteTestResultsToExcel(resultFilePath, null);
            }


        }

    }
}
