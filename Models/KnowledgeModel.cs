using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Models
{
    public class KnowledgeModel
    {
        public List<string> knowledgeStrings { get; set; } = new();

        public List<string> propositionSymbols { get; set; } = new();

        public string query { get; set; } = "";

        //Debug functions.
        public void printData()
        {
            Console.WriteLine("-------------------------------");

            Console.WriteLine("Knowledge Strings: ");
            Console.WriteLine(string.Join(Environment.NewLine, knowledgeStrings.Select(m => $"[{m}]")));

            Console.WriteLine();

            Console.WriteLine("Propoisiton Symbols: ");
            Console.WriteLine(string.Join(Environment.NewLine, propositionSymbols.Select(m => $"[{m}]")));

            Console.WriteLine();

            Console.WriteLine("Query: ");
            Console.WriteLine(query);

            Console.WriteLine("-------------------------------");
        }
    }
}
