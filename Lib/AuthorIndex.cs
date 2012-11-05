using System;

namespace Lib
{
    public class AuthorIndex
    {
        private const int MaxAuthors = 100;

        private readonly string[] _authors = new string[MaxAuthors];

        public int this[string name]
        {
            get { return IndexForAuthor(name); }
        }

        public int IndexForAuthor(string name)
        {
            for (var i = 1; i < MaxAuthors; i++)
            {
                if (_authors[i] == name)
                {
                    return i;
                }
                if(_authors[i] == null)
                {
                    _authors[i] = name;
                    return i;
                }
            }
            throw new Exception(String.Format("Too many authors; only supports {0}.", MaxAuthors));
        }
    }
}
