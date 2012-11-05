using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Lib
{
    public class SvnParser
    {
        private static readonly Regex UnwantedTypeRegex = new Regex("\\.(bmp|png|jpg|gif|jpeg|ico|dll|so)$");

        public readonly Dictionary<String, FilePaths> authorCommitCount = new Dictionary<String, FilePaths>();
        public readonly FilePaths totalCommitCount = new FilePaths();

        public void ParseFile(string fileName)
        {
            XElement svnDoc = XElement.Load(fileName);

            foreach (XElement logEntry in svnDoc.Elements())
            {
                XElement authorElement = logEntry.Element("author");
                if(authorElement == null)
                    continue;
                
                string name = authorElement.Value;
                if (!authorCommitCount.ContainsKey(name)) authorCommitCount.Add(name, new FilePaths());

                foreach (XElement descendant in logEntry.Descendants("path"))
                {
                    string path = descendant.Value;
                    if (!UnwantedTypeRegex.IsMatch(path))
                    {
                        string action = descendant.Attribute("action").Value;

                        if ("M" == action || "A" == action)
                        {
                            totalCommitCount.IncrementValue(path);
                            authorCommitCount[name].IncrementValue(path);
                        }
                    }
                }
            }
        }
    }
}
