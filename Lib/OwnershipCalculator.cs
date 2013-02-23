using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib
{
    public class OwnershipCalculator
    {
        private const decimal COMMIT_THRESHOLD = 0.50M;

        public readonly Dictionary<string, List<int>> OwnershipDistribution = new Dictionary<string, List<int>>();

        public readonly FilePaths codeDistribution = new FilePaths();
        public readonly AuthorIndex _authorIndex = new AuthorIndex();

        public void Calculate(Dictionary<String, FilePaths> authors, FilePaths totalCommits)
        {
            InitializeAuthorIndex(authors);

            foreach (string path in totalCommits.Keys)
            {
                int pathCommitCount = Convert.ToInt32(totalCommits[path]);

                Dictionary<string, int> listOfAuthors = GetListOfAuthors(path, authors);

                CalculateNumberOfAuthors(path, listOfAuthors, pathCommitCount, COMMIT_THRESHOLD);
            }


            GetSourceCodeDirectories(codeDistribution);

            foreach (string directory in OwnershipDistribution.Keys)
            {
                foreach (string path in codeDistribution.Keys)
                {
                    if (DirectoryComparison(path, directory))
                        OwnershipDistribution[directory].Add(codeDistribution[path]);
                }
            }

            RemoveEmptyDirectories();
        }

        private void InitializeAuthorIndex(Dictionary<String, FilePaths> authors)
        {
            // We're running over the authors sorted by their total commits, so that the index assigns
            // lower numbers for authors with more commits.
            foreach (var p in authors.OrderByDescending(p => p.Value.Total))
                _authorIndex.IndexForAuthor(p.Key);
        }

        private void RemoveEmptyDirectories()
        {
            List<KeyValuePair<string, List<Int32>>> keyValuePairs =
                OwnershipDistribution.Where(pair => pair.Value.Count == 0).ToList();

            foreach (var keyValuePair in keyValuePairs)
            {
                OwnershipDistribution.Remove(keyValuePair.Key);
            }
        }

        private static Dictionary<string, int> GetListOfAuthors(object path, Dictionary<String, FilePaths> authors)
        {
            var listOfAuthors = new Dictionary<string, int>();
            foreach (string key in authors.Keys)
            {
                if (authors[(string) key].ContainsKey((string) path))
                    listOfAuthors.Add(key, Convert.ToInt32(authors[key][(string) path]));
            }
            return listOfAuthors;
        }

        public void CalculateNumberOfAuthors(string path, Dictionary<string, int> listOfAuthors, int pathCommitCount, decimal commitThreshold)
        {
            IOrderedEnumerable<KeyValuePair<string, int>> orderedList =
                listOfAuthors.OrderByDescending(valuePair => valuePair.Value);

            if (orderedList.First().Value >= pathCommitCount*commitThreshold)
                codeDistribution[path] = _authorIndex[orderedList.First().Key];
            else
                codeDistribution[path] = 0;
        }


        private bool DirectoryComparison(object path, string directory)
        {
            if (!path.ToString().Contains("."))
                return false; //is a directory

            //TODO: Perhaps RegEx is a better way to compare
            int lastDir = path.ToString().LastIndexOf("/");
            string trimmedPath = path.ToString().Remove(lastDir);

            return directory.CompareTo(trimmedPath) == 0;
        }

        private void GetSourceCodeDirectories(FilePaths codeDistribution)
        {
            foreach (string path in codeDistribution.Keys)
            {
                int lastDir = 0;
                string trimmedPath = path;
                while (lastDir != -1 && trimmedPath.Length != 0)
                {
                    lastDir = trimmedPath.LastIndexOf("/");
                    trimmedPath = path.Remove(lastDir);
                    if (!OwnershipDistribution.ContainsKey(trimmedPath))
                        OwnershipDistribution.Add(trimmedPath, new List<int>());
                }
            }
        }
    }
}