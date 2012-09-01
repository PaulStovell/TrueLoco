using System;
using System.Collections.Generic;

namespace TrueLoco.Http
{
    public class PostDictionary : Dictionary<string, string>
    {
        public PostDictionary() : base((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}