using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inference_Engine.Tools
{
    public class FileParser
    {
        //Create new knowledge model.
        private KnowledgeModel model = new();

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
            MatchCollection symbolMatches = Regex.Matches(tellLine, @"[a-z]\d*");

            foreach (Match m in symbolMatches)
            {
                if (!model.symbols.Contains(m.Value))
                {
                    model.symbols.Add(m.Value.Trim());
                }
            }
        }

        public void parseKnowledgeStrings(string tellLine)
        {
            foreach (string line in tellLine.Split(';'))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    model.sentences.Add(line.Trim());
                }
            }
        }
    }
}
