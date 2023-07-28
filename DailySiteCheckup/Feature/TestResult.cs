using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailySiteCheckup.Feature
{
    public class TestResult
    {
        public string SiteName { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string TestCaseName { get; set; }
    }
    public class ReadMailsac
    {
        public string owner { get; set; }
        public string forward { get; set; }
        public string enablews { get; set; }
        public string webhook { get; set; }
        public string webhookSlack { get; set; }
        public string webhookSlackToFrom { get; set; }
        public string catchAll { get; set; }
        public string password { get; set; }
        public string info { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        [JsonPropertyName("_id")]
        public string Id { get; set; }
    }
    public class InboxList
    {
        public string subject { get; set; }
        [JsonPropertyName("_id")]
        public string MessageId { get; set; }
    }
    public class EmailBody
    {
        public string HtmlContent { get; set; }
    }
    public class EmailCounter
    {
        public int counter { get; set;}
    }
}
