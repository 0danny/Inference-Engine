using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inference_Engine.Tools
{
    public class FileParser
    {
        //Create new knowledge model.
        private KnowledgeModel model = new KnowledgeModel();

        public KnowledgeModel parseFile(string fileName)
        {
            //Read each line of the file.
            string[] fileLines = File.ReadAllLines(fileName);

            //Loop through each line of file.
            for(int i = 0; i < fileLines.Length; i++)
            {
                //Only check lines that are not empty.
                if (!string.IsNullOrEmpty(fileLines[i]))
                {
                    //If the current line == 'tell' then the next line will be the string we need.
                    if (fileLines[i].ToLower() == "tell")
                    {
                        string tellLine = fileLines[i + 1];

                        model.knowledgeBase = tellLine;

                        parseKnowledgeStrings(tellLine);

                        parsePropositonSymbols(tellLine);
                    }

                    //If the current line == 'ask' then the next line is teh query string.
                    if (fileLines[i].ToLower() == "ask")
                    {
                        model.query = fileLines[i + 1].Trim();
                    }
                }
            }

            //Return the model
            return model;
        }

        private void parsePropositonSymbols(string tellLine)
        {
            MatchCollection symbolMatches = Regex.Matches(tellLine, @"[a-z]\d*"); // Find matches for proposition symbols using a regular expression

            foreach (Match m in symbolMatches)
            {
                string symbol = m.Value.Trim(); // Get the matched symbol

                // If the symbol is not already present in the model's symbols list, add it
                if (!model.symbols.Contains(symbol))
                {
                    model.symbols.Add(symbol);
                }
            }
        }

        public void parseKnowledgeStrings(string tellLine)
        {
            foreach (string line in tellLine.Split(';')) // Split the tell line into individual strings separated by semicolons
            {
                if (!string.IsNullOrEmpty(line)) // If the line is not empty or null
                {
                    model.sentences.Add(line.Trim()); // Add the trimmed line to the model's sentences list
                }
            }
        }
    }
}
