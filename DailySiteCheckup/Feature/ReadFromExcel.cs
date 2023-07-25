using ExcelDataReader;
using OfficeOpenXml;
using System;
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
        public ReadFromExcel()
        {
            groupedData = new Dictionary<string, List<string>>();
        }

        public void GroupSiteandColumns(string filePath)
        {
            // Load the Excel file
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Get the first worksheet in the Excel file
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                string worksheet_name = package.Workbook.Worksheets[1].Name;
                // Find the dimensions of the worksheet
                int rowCount = worksheet.Dimension.Rows;
                int columnCount = worksheet.Dimension.Columns;
                //Determine the column index to group by
                int groupByColumnIndex = 1;
                //Iterate through each row
                for (int row = 2; row <= rowCount; row++)
                {
                    // Get the value of the group by column
                    var groupByKey = worksheet.Cells[row, groupByColumnIndex].Value?.ToString();

                    // Skip if the group by column is empty
                    if (string.IsNullOrEmpty(groupByKey))
                        continue;

                    // Get the value of the data column
                    var dataValue = worksheet.Cells[row, groupByColumnIndex + 1].Value?.ToString();

                    // Check if the group by key already exists in the dictionary
                    if (groupedData.ContainsKey(groupByKey))
                    {
                        // Add the data value to the existing group
                        groupedData[groupByKey].Add(dataValue);
                    }
                    else
                    {
                        // Create a new group with the data value
                        groupedData[groupByKey] = new List<string> { dataValue };
                    }
                }

                // Iterate through the grouped data and print the results
                foreach (var group in groupedData)
                {
                    string groupKey = group.Key;
                    List<string> dataValues = group.Value;

                    Console.WriteLine($"Group: {groupKey}");
                    Console.WriteLine("Data Values: " + string.Join(", ", dataValues));
                    Console.WriteLine();
                }
            }

        }
    }
}
