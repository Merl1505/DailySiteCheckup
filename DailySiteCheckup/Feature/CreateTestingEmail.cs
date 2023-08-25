using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;

namespace DailySiteCheckup.Feature
{
    public class CreateTestingEmail
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        int counter_val = 0;
        public static string IsExcelEmpty { get; set; }

        public string GenerateTestingEmail()
        {
            // Build the configuration

            //string UpdateCounterFilepath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Reports\\UpdateCounter.txt";
            string UpdateCounterFilepath = "C:\\Users\\Merlin.Savarimuthu\\Reports\\EmailSetting\\UpdateCounter.txt";
            int counter = GetNextCounterValue(UpdateCounterFilepath);
            counter++;
            string testing_mail_id = configuration["email_prefix"] + counter + "@mailsac.com";
            UpdateEmailSettings(testing_mail_id,UpdateCounterFilepath, counter);

            return testing_mail_id;
        }
        public int GetNextCounterValue(string UpdateCounterFilepath)
        {
            // Check if the file exists
            if (File.Exists(UpdateCounterFilepath))
            {
                // Read all lines from the file and store them in an array
                string[] lines = File.ReadAllLines(UpdateCounterFilepath);

                foreach (string line in lines)
                {
                    if (line.Contains("current_counter"))
                    {
                        string[] currentCounter = line.Split(':');
                       counter_val = Convert.ToInt32(currentCounter[1]);
                       break;
                    }
                  
                }
            }
            else
            {
                Console.WriteLine("The file does not exist.");
            }
            return counter_val ;
        }
        public void UpdateEmailSettings(string testing_mail_id, string filePath, int current_counter)
        {
            try
            {
                // Create a new file or overwrite the existing one
                using (var fileStream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(fileStream))
                {
                   
                    writer.WriteLine("current_counter:"+ current_counter);
                    writer.WriteLine("last_generated_email:"+ testing_mail_id);
                }

                Console.WriteLine("Data has been written to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
