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

        public int PostGramLength { get; private set; }

        public NGram(string str)
        {
            ThisGram = str;
            PostGrams = new List<string>();
        }

        public string GetRandomPostGram()
        {
            Random ran = new Random();
            int index = ran.Next(PostGrams.Count);
            return PostGrams[index];
        }

        public void AddPostGram(string str)
        {
            PostGrams.Add(str);
            PostGramLength++;
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
