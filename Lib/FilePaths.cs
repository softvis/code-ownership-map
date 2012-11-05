using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lib
{
    public class FilePaths
    {
        // Keeping Hashtable for now to make sure output looks identical; 
        // Dictionary<String, Int32> hashes differently it seems.
        private readonly Hashtable _values = new Hashtable();

        public int this[string path]
        {
            get { return Convert.ToInt32(_values[path]); }
            set { _values[path] = value; }
        }

        public ICollection<string> Keys
        {
            get { return _values.Keys.Cast<string>().ToList(); }
        }

        public int Total
        {
            get { return _values.Values.Cast<int>().Sum(); }
        }

        public bool ContainsKey(String key)
        {
            return _values.ContainsKey(key);
        }

        public void IncrementValue(string path)
        {
            int current = 0;
            if (_values.ContainsKey(path))
            {
                current = Convert.ToInt32(_values[path]);
            }
            _values[path] = current + 1;
        }
    }
}