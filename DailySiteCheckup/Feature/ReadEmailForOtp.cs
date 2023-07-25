using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http.Headers;

namespace DailySiteCheckup.Feature
{
    public class ReadEmailForOtp
    {

        public async Task ReadMailsacEmailAPIAsync()
        {
            string apiKey = "k_pvZeVs1pfqIaEtZfvw6YrPSQE2g6FHIb3vjwZ";

            // Base URL for the Mailsac API
            string apiUrl = "https://mailsac.com/api";

            // Create the HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                // Set up HttpClient headers
                //httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                httpClient.DefaultRequestHeaders.Add("Mailsac-Key", apiKey);

                try
                {
                    //1. Get list of email addresses associated with your Mailsac account
                    string emailAddressesUrl = $"{apiUrl}/addresses";
                    HttpResponseMessage emailAddressesResponse = await httpClient.GetAsync(emailAddressesUrl);
                    emailAddressesResponse.EnsureSuccessStatusCode();
                    var emailAddresses = await emailAddressesResponse.Content.ReadAsStringAsync();
                    var emailList = System.Text.Json.JsonSerializer.Deserialize<ReadMailsac[]>(emailAddresses);

                    // Assuming you want to read emails from the first email address in the list
                    string emailAddress = emailList[0].Id;

                    // 2. Get list of emails for the chosen email address
                    string emailsUrl = $"{apiUrl}/addresses/{emailAddress}/messages";
                    HttpResponseMessage emailsResponse = await httpClient.GetAsync(emailsUrl);
                    emailsResponse.EnsureSuccessStatusCode();
                    string emailsJson = await emailsResponse.Content.ReadAsStringAsync();
                    var emailDetails = System.Text.Json.JsonSerializer.Deserialize<InboxList[]>(emailsJson);
                    string messageId = emailDetails[0].MessageId;

                    //3. call the get message html body end point
                    string messageURL = $"{apiUrl}/body/{emailAddress}/{messageId}";
                    HttpResponseMessage inboxEmailResponse = await httpClient.GetAsync(messageURL);
                    inboxEmailResponse.EnsureSuccessStatusCode();
                    string inboxEmailJson = await inboxEmailResponse.Content.ReadAsStringAsync();

                    EmailBody body = new EmailBody
                    {
                        HtmlContent = inboxEmailJson
                    };

                    //serialise the html raw structure to a string to get the otp 
                    string htmlBodyJson = JsonConvert.SerializeObject(body);
                    string text = htmlBodyJson.ToString();
                    string targetWord = "code is";
                    int targetIndex = text.IndexOf(targetWord, StringComparison.OrdinalIgnoreCase);
                    if (targetIndex != -1)
                    {
                        //process to get the otp
                        int nextWordStartIndex = text.IndexOfAny(new[] { ' ', '\t', '\n' }, targetIndex + targetWord.Length);
                        int nextWordEndIndex = text.IndexOfAny(new[] { ' ', '\t', '\n' }, nextWordStartIndex + 1);
                        string nextWord = nextWordEndIndex == -1
                           ? text.Substring(nextWordStartIndex + 1)
                           : text.Substring(nextWordStartIndex + 1, nextWordEndIndex - nextWordStartIndex - 1);
                        string EmailOTP = nextWord;
                        TestContext.Progress.WriteLine("Email OTP is..." + nextWord);

                    }
                    else
                    {
                        TestContext.Progress.WriteLine("Email OTP is not found.....");

                    }

                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error reading emails: " + ex.Message);
                }
            }

        }

    }
}
