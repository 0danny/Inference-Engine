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

            TTQuery entails = checkEntailment(models, model);

            Console.WriteLine(entails.getEntailmentResponse());

            //testFunction(models[0], model);

            //printTruthTable(models, model);
        }

        public void testFunction(Dictionary<string, bool> model1, KnowledgeModel model)
        {
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

            foreach (string symbol in model.symbols)
            {
                Console.Write($"{model1[symbol]}\t");
            }

            foreach (string knowledgeString in model.sentences)
            {
                //Console.Write($"{evaluateExpression(knowledgeString, model1)}\t");
            }

            Console.WriteLine(evaluateExpression("(a <=> (c => ~d)) & b & (b => a)", model1));
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

            foreach (string sentence in knowledgeModel.sentences)
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


        private TTQuery checkEntailment(List<Dictionary<string, bool>> models, KnowledgeModel knowledgeModel)
        {
            List<bool> temp_array = new();

            int counter = 0;

            foreach (Dictionary<string, bool> m in models)
            {
                //Compare the query with the knowledge base.

                bool KB = evaluateKnowledgeBase(m, knowledgeModel);

                bool query = m[knowledgeModel.query];

                if (KB)
                {
                    temp_array.Add(query);

                    counter++;
                }
            }

            bool result = temp_array.All(x => x);

            return new TTQuery(result, counter);
        }

        private string removeBraces(string theString)
        {
            return theString.Replace("(", "").Replace(")", "");
        }

        private bool evaluateExpression(string expression, Dictionary<string, bool> model)
        {
            // split the expression into parts by the main operator
            string[] parts;

            if (expression.Contains("=>"))
            {
                parts = expression.Split(new string[] { "=>" }, 2, StringSplitOptions.TrimEntries);

                //Console.WriteLine($"Operator: => | Left: {parts[0]} | Right: {parts[1]}");

                bool a = evaluateExpression(parts[0], model);

                bool b = evaluateExpression(parts[1], model);

                return (!a || b);
            }
            else if (expression.Contains("&"))
            {
                parts = expression.Split(new string[] { "&" }, 2, StringSplitOptions.TrimEntries);

                //Console.WriteLine($"Operator: & | Left: {parts[0]} | Right: {parts[1]}");

                return evaluateExpression(parts[0], model) && evaluateExpression(parts[1], model);
            }
            else
            {
                expression = removeBraces(expression);

                //Console.WriteLine($"Operator: Checking Symbol | Expression: {expression}");

                // symbol by itself, look it up in the models dictionary.
                string symbol = expression.Trim();
                return model.ContainsKey(symbol) && model[symbol];
            }
        }
    }
}
