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

        public void WriteTestResultsToExcel(string filePath, List<TestResult> testResults)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Test Results");

                // Set the column headers
                worksheet.Cells[1, 1].Value = "TestCase Name";
                worksheet.Cells[1, 2].Value = "Status";
                worksheet.Cells[1, 3].Value = "Message";
                worksheet.Cells[1, 3].Value = "Site Name";

                // Write test results to the worksheet
                for (int i = 0; i < testResults.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = testResults[i].TestCaseName;
                    worksheet.Cells[i + 2, 2].Value = testResults[i].Status;
                    worksheet.Cells[i + 2, 3].Value = testResults[i].Message;
                    worksheet.Cells[i + 2, 4].Value = testResults[i].SiteName;
                }

                // Save the Excel package to a file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }


        }

    }
}
