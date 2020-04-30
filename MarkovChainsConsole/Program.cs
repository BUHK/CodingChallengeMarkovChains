using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarkovChainsConsole
{
    class Program
    {
        //the text to perform work on
        static string text = "";

        static int nGramLength = 3;
        static int addLength = 1;

        static List<NGram> nGrams;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            text = "the theremin is theirs, ok? yes, it is. this is a theremin.";

            SanitizeText();

            GenerateNGrams();

            foreach (var item in nGrams)
            {
                Console.WriteLine(item);
            }

            //MarkovIt();

            Console.ReadLine();
        }

        /**
         * generate new text
         */
        private static void MarkovIt()
        {
            throw new NotImplementedException();
        }

        /**
         * parse the text into n-grams
         */
        private static void GenerateNGrams()
        {
            nGrams = new List<NGram>();
            for (int i = 0; i <= (text.Length - addLength) - nGramLength; i++)
            {
                string str = text.Substring(i, nGramLength);
                NGram currentGram = new NGram(str);

                //if new, add substring of nGramLength to nGram list
                if (!nGrams.Contains(currentGram))
                {
                    nGrams.Add(currentGram);
                }
                //else set current gram to existing gram
                else
                {
                    currentGram = nGrams.Find(x => x.Equals(currentGram));
                }

                //add next char to next char possibility list
                currentGram.AddPostGram(text.Substring(i + nGramLength, addLength));
            }
        }

        /**
         * sanitize the input text
         */
        private static void SanitizeText()
        {
            text = text.Trim();
            text = text.PadRight(addLength);
        }
    }
}
