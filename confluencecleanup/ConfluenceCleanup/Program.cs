using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConfluenceCleanup
{
    class Program
    {
        static string findString(string input, string start, string end)
        {
            if (input.Contains(end)) {
                string result = Regex.Match(input.ToString(), start+"(.+?)"+end).Groups[1].Value;
                return result.Replace("<![CDATA[", "").Replace("]]>", "");

            } else
            {
                return "";
            }
        }

        static void Main(string[] args)
        {
            List<string[]> folderList = new List<string[]>();
            var filename = "entities.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDirectory, filename);

            //var tags = from attachments in XDocument.Load(path)
            //                                     .Element("hibernate-generic").Elements("object")
            //           where attachments.Attribute("class").Value == "Page"
            //           select new { ID = attachments.Element("id").Value, Name = attachments.Element("property").Value };

            string raw = File.ReadAllText(path);
            var regex = new Regex("(<object class=\"Page\").+?(</object>)", RegexOptions.Singleline);
            var rawData = regex.Matches(raw);

            Console.WriteLine(rawData.Count);
            // Get the main page/folder names and IDs
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in rawData)
            {
                string data = item.ToString();
                string masterId = findString(data, "<id name=\"id\">","</id>");
                string title = findString(data, "<property name=\"title\">","</property>");
                dict.Add(masterId, title);
            }

            Console.WriteLine(dict["258048207"]);

            // We have a dict of master folders, now loop through attachments and populate dict matching to relevant folders


            //foreach (KeyValuePair<string, string> item in dict)
            //{
            //    Console.WriteLine("ID: {0}, Title: {1}", item.Key, item.Value);
            //}

            //var filename = "students.xml";
            //var currentDirectory = Directory.GetCurrentDirectory();
            //var path = Path.Combine(currentDirectory, filename);

            //var names = from student in XDocument.Load(path)
            //                                     .Element("Students").Elements("Student")
            //                            where (int)student.Element("TotalMarks") > 800
            //                            orderby(int)student.Element("TotalMarks") descending
            //                            select new { ID = student.Attribute("Id").Value, Name = student.Element("Name").Value };
            //foreach (var name in names)
            //{
            //    Console.WriteLine($"{name.Name} - {name.ID}");
            //}

            Console.ReadLine();

        }
    }
}
