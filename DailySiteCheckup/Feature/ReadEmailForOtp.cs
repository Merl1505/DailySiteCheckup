using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http.Headers;
using HtmlAgilityPack;
using System.Reflection.Metadata;

namespace DailySiteCheckup.Feature
{
    public class ReadEmailForOtp
    {
        // Build the configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        public async Task<string> ReadMailsacEmailAPIAsync(string emailId)
        {
            string apiKey = configuration["ApiKey"];
            string EmailOTP = "";
            // Base URL for the Mailsac API
            string apiUrl = configuration["ApiUrl"]; ;

            // Create the HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                // Set up HttpClient headers
                //httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                httpClient.DefaultRequestHeaders.Add("Mailsac-Key", apiKey);

                try
                {
                    /* 1. Get list of email addresses associated with your Mailsac account
                    (temporarily not required)
                    string emailAddressesUrl = $"{apiUrl}/addresses";
                    HttpResponseMessage emailAddressesResponse = await httpClient.GetAsync(emailAddressesUrl);
                    emailAddressesResponse.EnsureSuccessStatusCode();
                    var emailAddresses = await emailAddressesResponse.Content.ReadAsStringAsync();
                    var emailList = System.Text.Json.JsonSerializer.Deserialize<ReadMailsac[]>(emailAddresses);

                    // Assuming you want to read emails from the first email address in the list
                    string emailAddress = emailList[0].Id;   (temporarily not required) */

                    // 2. Get list of emails for the chosen email address
                    string emailsUrl = $"{apiUrl}/addresses/{emailId}/messages";
                    HttpResponseMessage emailsResponse = await httpClient.GetAsync(emailsUrl);
                    emailsResponse.EnsureSuccessStatusCode();
                    string emailsJson = await emailsResponse.Content.ReadAsStringAsync();
                    var emailDetails = System.Text.Json.JsonSerializer.Deserialize<InboxList[]>(emailsJson);
                    string messageId = emailDetails[0].MessageId;

                    //3. call the get message html body end point
                    string messageURL = $"{apiUrl}/body/{emailId}/{messageId}";
                    HttpResponseMessage inboxEmailResponse = await httpClient.GetAsync(messageURL);
                    inboxEmailResponse.EnsureSuccessStatusCode();
                    string inboxEmailJson = await inboxEmailResponse.Content.ReadAsStringAsync();

                    EmailBody body = new EmailBody
                    {
                        HtmlContent = inboxEmailJson
                    };

                    //serialise the html raw structure to a string to get the otp 
                    string htmlBodyJson = JsonConvert.SerializeObject(body);
                    // Create a document object
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlBodyJson);

                    // Get the list of h1 elements
                    var h1Elements = doc.DocumentNode.Descendants("h1").ToList();
                    // Get the second h1 element
                    var secondH1Element = h1Elements[1];
                    string text = htmlBodyJson.ToString();
                    int inputLength = secondH1Element.InnerHtml.Length;
                    EmailOTP = secondH1Element.InnerHtml.Substring(inputLength - 4);

                    //int targetIndex = text.IndexOf(targetWord, StringComparison.OrdinalIgnoreCase);
                    //if (targetIndex != -1)
                    //{
                    //    //process to get the otp
                    //    int nextWordStartIndex = text.IndexOfAny(new[] { ' ', '\t', '\n' }, targetIndex + targetWord.Length);
                    //    int nextWordEndIndex = text.IndexOfAny(new[] { ' ', '\t', '\n' }, nextWordStartIndex + 1);
                    //    string nextWord = nextWordEndIndex == -1
                    //       ? text.Substring(nextWordStartIndex + 1)
                    //       : text.Substring(nextWordStartIndex + 1, nextWordEndIndex - nextWordStartIndex - 1);
                    //    EmailOTP = nextWord;
                    //}
                    //else
                    //{
                    //    TestContext.Progress.WriteLine("Email OTP is not found.....");

                    //}

                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error reading emails: " + ex.Message);
                }
            }
            return await Task.Run(() => { return EmailOTP; });
        }

    }
}
