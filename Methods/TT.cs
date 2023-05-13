using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
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
            List<Dictionary<string, bool>> models = generateModels(model.symbols);

            EntailmentQuery entails = checkEntailment(models, model);

            Console.WriteLine(entails.getEntailmentResponse());
        }

        public void printTruthTable(List<Dictionary<string, bool>> models, KnowledgeModel model)
        {
            // Print the header row
            Console.Write("Model\t");

            foreach (string symbol in model.symbols)
            {
                Console.Write($"{symbol}\t");
            }

            foreach (string knowledgeString in model.sentences)
            {
                Console.Write($"{knowledgeString}\t");
            }

            Console.Write($"{model.getKB()}\t");

            Console.WriteLine();

            // Print each row of the truth table
            int modelNumber = 1;

            foreach (Dictionary<string, bool> m in models)
            {
                Console.Write($"M{modelNumber++}\t");

                foreach (string symbol in model.symbols)
                {
                    Console.Write($"{m[symbol]}\t");
                }

                foreach (string knowledgeString in model.sentences)
                {
                    Console.Write($"{evaluateExpression(knowledgeString, m)}\t");
                }
                Console.Write($"{evaluateKnowledgeBase(m, model)}\t");

                Console.WriteLine();
            }
        }

        public bool evaluateKnowledgeBase(Dictionary<string, bool> model, KnowledgeModel knowledgeModel)
        {
            List<bool> temp_array = new();

            foreach(string sentence in knowledgeModel.unfilteredSentences)
            {
                temp_array.Add(evaluateExpression(sentence, model));
            }

            return temp_array.All(x => x);
        }

        private List<Dictionary<string, bool>> generateModels(List<string> symbols)
        {
            // Initialize the queue with an empty model.
            Queue<Dictionary<string, bool>> queue = new();

            queue.Enqueue(new Dictionary<string, bool>());

            foreach (string symbol in symbols)
            {
                // Make a temporary queue to hold the extended models.
                Queue<Dictionary<string, bool>> tempQueue = new();

                while (queue.Count > 0)
                {
                    // Remove the next model from the queue.
                    Dictionary<string, bool> model = queue.Dequeue();

                    // Extend the model with the current symbol being true and false.
                    Dictionary<string, bool> modelFalse = new(model);
                    modelFalse[symbol] = false;

                    Dictionary<string, bool> modelTrue = new(model);
                    modelTrue[symbol] = true;

                    tempQueue.Enqueue(modelFalse);

                    tempQueue.Enqueue(modelTrue);
                }

                // Replace the original queue with the temporary queue.
                queue = tempQueue;
            }

            // Convert the queue to a list and return it.
            return queue.ToList();
        }

        private EntailmentQuery checkEntailment(List<Dictionary<string, bool>> models, KnowledgeModel knowledgeModel)
        {
            List<bool> temp_array = new();

            int counter = 0;

            foreach (Dictionary<string, bool> m in models)
            {
                //Compare the query with the knowledge base.

                bool KB = evaluateKnowledgeBase(m, knowledgeModel);

                bool query = m[knowledgeModel.query];

                if(KB)
                {
                    temp_array.Add(query);

                    counter++;
                }
            }

            bool result = temp_array.All(x => x);

            return new EntailmentQuery(result, counter);
        }

        private bool evaluateExpression(string expression, Dictionary<string, bool> model)
        {
            // split the expression into parts by the main operator
            string[] parts;

            // => is only false when A is true and B is false.

            if (expression.Contains("=>"))
            {
                parts = expression.Split(new string[] { "=>" }, StringSplitOptions.None);

                bool a = evaluateExpression(parts[0], model);

                bool b = evaluateExpression(parts[1], model);

                return (a && !b) ? false : true;
            }
            else if (expression.Contains("&"))
            {
                parts = expression.Split(new string[] { "&" }, StringSplitOptions.None);

                return evaluateExpression(parts[0], model) && evaluateExpression(parts[1], model);
            }
            else
            {
                // symbol by itself, look it up in the models dictionary.
                string symbol = expression.Trim();
                return model.ContainsKey(symbol) && model[symbol];
            }
        }

        private class EntailmentQuery
        {
            private bool response { get; set; } = false;
            private int symbolsEntailed { get; set; } = 0;

            public EntailmentQuery(bool response, int symbolsEntailed)
            {
                this.response = response;
                this.symbolsEntailed = symbolsEntailed;
            }

            public string getEntailmentResponse()
            {
                if (!response)
                {
                    return "NO";
                }
                else
                {
                    return $"YES: {symbolsEntailed}";
                }
            }
        }
    }
}
