using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Methods
{
    public class TT : Method
    {
        public TT()
        {
            name = "Truth Table Checking";
            shortName = "TT";
        }

        public override void runMethod(KnowledgeModel model)
        {
            //initialise and create the truth table for all symbols
            int[,] truthTable;
            truthTable = createBinaryTable(model.propositionSymbols);

            //evaluate truth values for all sentences
            evaluateSentences(truthTable, model.knowledgeStrings);
        }

        public int[,] createBinaryTable(List<string> symbols)
        {
            //calculate number of rows
            int rows = (int)Math.Pow(2, symbols.Count);

            //initialise table
            int[,] ttable = new int[rows, symbols.Count];

            for (int i = 0; i < rows; i++)
            {
                int rownumber = i;
                //for each row, take the row number and convert to binary, then input each bit into the table.

                for(int j  = 0; j < symbols.Count; j++)
                {
                    int bit = rownumber % 2; //find least significant bit
                    rownumber /= 2; //shift bits to the right
                    ttable[i, j] = bit; //input least significant bit
                }
            }

            return ttable;
        }

        public void evaluateSentences(int[,] ttable, List<string> strings)
        {
            //loop through each sentence
            for (int i = 0; i < strings.Count; ++i)
            {
                //set the current sentence
                string sentence = strings[i];

                //evaluate that sentence for each combination in the truth table
                evaluateSentence(ttable, sentence);
            }
        }

        public int[] evaluateSentence(int[,] ttable, string sentence)
        {
            int rows = ttable.GetLength(0);
            int columns = ttable.GetLength(1);

            int[] results = new int[rows];

            //loop through each row in the truth table and add truth value to results
            for(int i = 0; i < rows; ++i)
            {
                int[] rowValues = new int[columns];

                for(int j = 0; j < columns; j++)
                {
                    rowValues[j] = ttable[i, j];
                }

                results[i] = truthValue(sentence, rowValues);
            }

            return results;
        }

        public int truthValue(string sentence, int[] rowValues)
        {
            //evaluate whether sentence is true according to the truth values set and return true or false
            return 0;
        }
    }
}
