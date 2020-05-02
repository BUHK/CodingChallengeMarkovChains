using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MarkovChainsConsole
{
    class Program
    {
        //the text to perform work on
        private static string text = "";
        private static int nGramLength = 6;
        //the length of next gram that will be added during markov chain generation
        private static int addLength = 1;
        private static int markovChainLegnth = 50;

        static List<NGram> nGrams;

        static void Main(string[] args)
        {
            text = File.ReadAllText(@"../../../inputText.txt");
            //Console.WriteLine(text);
            markovChainLegnth = text.Length;

            SanitizeText();

            GenerateNGrams();

            //foreach (var item in nGrams)
            //{
            //    Console.WriteLine(item);
            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    string markovChain = MarkovIt(nGrams[0], markovChainLegnth);
            //    Console.WriteLine(markovChain); 
            //}

            string markovChain = MarkovIt(nGrams[0], markovChainLegnth);
            Console.WriteLine(markovChain);
            Console.WriteLine($"Input text length: {text.Length}");
            Console.WriteLine($"Markov chain legnth: {markovChain.Length}");

            Console.ReadLine();
        }

        /// <summary>
        /// Generate new text by Markov chain 
        /// </summary>
        /// <param name="startingGram">The manually chosen start of the new text.</param>
        /// <param name="chainLegnth">The number of times new text will be added to the starting gram.</param>
        /// <returns></returns>
        private static string MarkovIt(NGram startingGram, int chainLegnth)
        {
            string markovChain = "";
            markovChain += startingGram.ThisGram;

            for (int i = 0; i < chainLegnth; i++)
            {
                string newChar = startingGram.GetRandomPostGram();
                markovChain += newChar;
                NGram currentGram = new NGram(GetLastNGram(markovChain));
                startingGram = nGrams.Find(x => x.Equals(currentGram));
                //the only way startingGram should be null is if the last nGram was selected.
                //  in which case I, nor the CodingTrain guy, have a better solution than ending the chain early
                if (startingGram == null)
                {
                    break;
                }
            }

            return markovChain;
        }

        private static string GetLastNGram(string text)
        {
            string last = text.Substring(text.Length - nGramLength);

            return last;
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
            //text = text.PadRight(text.Length + 1);
        }
    }
}
