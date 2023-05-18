using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Models
{
    public class KnowledgeModel
    {
        public List<string> sentences { get; set; } = new();

        public List<string> symbols { get; set; } = new();

        public string knowledgeBase { get; set; } = "";

        public string query { get; set; } = "";

        //Debug functions.
        public string getKB()
        {
            return $"{knowledgeBase.Replace(";", " ^")}";
        }

        public void printData()
        {
            Console.WriteLine("-------------------------------");

            Console.WriteLine("Knowledge Strings: ");
            Console.WriteLine(string.Join(Environment.NewLine, sentences.Select(m => $"[{m}]")));

            Console.WriteLine();

            Console.WriteLine("Whole KB: ");
            Console.WriteLine(knowledgeBase);

            Console.WriteLine();

            Console.WriteLine("Proposition Symbols: ");
            Console.WriteLine(string.Join(Environment.NewLine, symbols.Select(m => $"[{m}]")));

            Console.WriteLine();

            Console.WriteLine("Query: ");
            Console.WriteLine(query);

            Console.WriteLine("-------------------------------");
        }
    }
}
