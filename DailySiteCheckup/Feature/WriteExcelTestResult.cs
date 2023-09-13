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

        public void WriteTestResultsToExcel(string filePath, List<TestResult> testResults)
        {
            DateTime currentDate = DateTime.Now;
            string currdate = currentDate.ToShortDateString();
            string sheetdate = currdate.Replace("/", "");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //List<TestResult> testResults = new List<TestResult>
            //        {
            //            new TestResult { SiteURL = "https://jeffersonwells.no", SiteName = "NO_Jefferson wells", SignUpStatus = "Y",LoginStatus=null, Message = "Success", TestCaseName = "Signup" }
            //              };
            // Load your Excel file into a FileInfo object
            FileInfo file = new FileInfo(filePath);

            // Create a new Excel package
            using (var package = new ExcelPackage(file))
            {
                // Create a new worksheet
                //var worksheet = package.Workbook.Worksheets.Add("TestResults_" + sheetdate);
                var worksheet = package.Workbook.Worksheets.SingleOrDefault(ws => ws.Name.Trim() == "TestResults_" + sheetdate);
                if (worksheet != null)
                {
                    int lastRow = worksheet.Dimension.End.Row;
                    int lastColumn = worksheet.Dimension.End.Column;
                    // Write test results to the worksheet
                    for (int i = 0; i < testResults.Count; i++)
                    {
                        worksheet.Cells[lastRow + 1, 1].Value = testResults[i].SiteName;
                        worksheet.Cells[lastRow + 1, 2].Value = testResults[i].SiteURL;
                        worksheet.Cells[lastRow + 1, 3].Value = testResults[i].SignUpStatus;
                        worksheet.Cells[lastRow + 1, 4].Value = testResults[i].LoginStatus == null ? "N" : testResults[i].LoginStatus;
                        worksheet.Cells[lastRow + 1, 5].Value  = testResults[i].ForgotPwdStatus == null ? "N" : testResults[i].ForgotPwdStatus;
                        worksheet.Cells[lastRow + 1, 6].Value  = testResults[i].ProfileEditStatus == null ? "N" : testResults[i].ProfileEditStatus;
                        worksheet.Cells[lastRow + 1, 7].Value  = testResults[i].UpdatePhoneStatus == null ? "N" : testResults[i].UpdatePhoneStatus;
                        worksheet.Cells[lastRow + 1, 8].Value = testResults[i].UpdatePwdStatus == null ? "N" : testResults[i].UpdatePwdStatus;
                    }
                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add("TestResults_"+sheetdate);
                    worksheet.Cells[1, 1].Value = "Sites";
                    worksheet.Cells[1, 2].Value = "URL";
                    worksheet.Cells[1, 3].Value = "Signup";
                    worksheet.Cells[1, 4].Value = "Login";
                    worksheet.Cells[1, 5].Value = "ForgotPassword";
                    worksheet.Cells[1, 6].Value = "Profile Edit";
                    worksheet.Cells[1, 7].Value = "Update Phone Number";
                    worksheet.Cells[1, 8].Value = "Update Password";

                    // Write test results to the worksheet
                    for (int i = 0; i < testResults.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = testResults[i].SiteName;
                        worksheet.Cells[i + 2, 2].Value = testResults[i].SiteURL;
                        worksheet.Cells[i + 2, 3].Value = testResults[i].SignUpStatus;
                        worksheet.Cells[i + 2, 4].Value = testResults[i].LoginStatus == null ? "N" : testResults[i].LoginStatus;
                        worksheet.Cells[i + 2, 5].Value = testResults[i].ForgotPwdStatus == null ? "N" : testResults[i].ForgotPwdStatus;
                        worksheet.Cells[i + 2, 6].Value = testResults[i].ProfileEditStatus == null ? "N" : testResults[i].ProfileEditStatus;
                        worksheet.Cells[i + 2, 7].Value = testResults[i].UpdatePhoneStatus == null ? "N" : testResults[i].UpdatePhoneStatus;
                        worksheet.Cells[i + 2, 8].Value = testResults[i].UpdatePwdStatus == null ? "N" : testResults[i].UpdatePwdStatus;
                    }
                }



                package.Save();
                // Save the Excel package to a file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
                //string resultFilePath = "C:\\Users\\Merlin.Savarimuthu\\Reports\\EmailSetting\\SiteCheck_TestResult.xlsx";
                //WriteTestResultsToExcel(resultFilePath, null);
            }



        }

    }
}
