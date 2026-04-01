using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://vkaushik2004.github.io/cse445_a4/NationalParks.xml";
        public static string xmlErrorURL = "https://vkaushik2004.github.io/cse445_a4/NationalParksErrors.xml";
        public static string xsdURL = "https://vkaushik2004.github.io/cse445_a4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            // Q3.1
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Q3.2
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Q3.3
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1 
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            StringBuilder errorMessages = new StringBuilder();

            try
            {
                string xsdContent = DownloadContent(xsdUrl);
                XmlSchemaSet schemaSet = new XmlSchemaSet();

                using (StringReader xsdReader = new StringReader(xsdContent))
                {
                    schemaSet.Add(null, XmlReader.Create(xsdReader));
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schemaSet;
                settings.ValidationType = ValidationType.Schema;

                settings.ValidationEventHandler += (sender, e) =>
                {
                    errorMessages.AppendLine(e.Message);
                };

                string xmlContent = DownloadContent(xmlUrl);
                using (StringReader xmlStringReader = new StringReader(xmlContent))
                using (XmlReader xmlReader = XmlReader.Create(xmlStringReader, settings))
                {
                    while (xmlReader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                errorMessages.AppendLine(ex.Message);
            }

            if (errorMessages.Length == 0)
                return "No errors are found";
            else
                return errorMessages.ToString().Trim();
        }

        // Q2.2 
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                string xmlContent = DownloadContent(xmlUrl);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

                return jsonText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
