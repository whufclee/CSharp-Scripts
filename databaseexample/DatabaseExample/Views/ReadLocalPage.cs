using System.Reflection;
using System.IO;
using Xamarin.Forms;
using DatabaseExample.Models;
using DatabaseExample.Views;
using System;
using System.Collections.Generic;

namespace DatabaseExample.Views
{
    public class ReadLocalPage :ContentPage
    {
        public string ReadLocalFile(string localFile)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ReadLocalPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream($"DatabaseExample.{localFile}.txt");
            string _content = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                _content = reader.ReadToEnd();
            }
            return _content;
        }

        public ReadLocalPage()
        {
            string text = ReadLocalFile("TR");
            var editor = new Label { Text = "loading..." };
            editor.Text = text;
            List<Articles> RSSItems = Views.AddLinkPage.RSSParser(text);
            foreach (var item in RSSItems)
            {
                Console.WriteLine(item.Title);
            }
        }
    }
}
