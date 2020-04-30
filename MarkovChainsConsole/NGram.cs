using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MarkovChainsConsole
{
    class NGram : IEquatable<NGram>
    {
        public string ThisGram { get; set; }
        private List<string> PostGrams { get; set; }

        public NGram (string str)
        {
            ThisGram = str;
            PostGrams = new List<string>();
        }

        public void AddPostGram(string str)
        {
            PostGrams.Add(str);
        }

        public override string ToString()
        {
            string str = ThisGram + ":";

            foreach (var item in PostGrams)
            {
                str += $" {item},";
            }

            return str;
        }

        //public override bool Equals(object other)
        //{
        //    NGram otherGram = other as NGram;

        //    return ThisGram.Equals(otherGram.ThisGram);
        //}

        public bool Equals(NGram other)
        {
            return ThisGram.Equals(other.ThisGram);
        }
    }
}
