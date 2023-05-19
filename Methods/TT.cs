using Inference_Engine.Models;
using Inference_Engine.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
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

            printTruthTable(models, model);
        }

        public void printModel(int index, List<Dictionary<string, bool>> models, KnowledgeModel model)
        {
            foreach (string symbol in model.symbols)
            {
                Console.Write($"{symbol}\t");
            }

            Console.WriteLine();
            foreach (string symbol in model.symbols)
            {
                Console.Write($"{models[index][symbol]}\t");
            }

            Console.WriteLine();
        }

        public void printTruthTable(List<Dictionary<string, bool>> models, KnowledgeModel model)
        {
            // Print the header row
            Console.Write("Model\t");

            foreach (string symbol in model.symbols)
            {
                Console.Write($"{symbol}\t");
            }

            Console.Write($"KB\t");

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

                bool query = evaluateExpression(knowledgeModel.query, m);

                if (KB)
                {
                    temp_array.Add(query);

                    counter++;
                }
            }

            bool result = temp_array.All(x => x);

            return new TTQuery(result, counter);
        }

        public bool evaluateExpression(string infixExpression, Dictionary<string, bool> model)
        {
            string postfixExpression = ShuntingYard.convertInfixToPostfix(infixExpression);

            Stack<bool> stack = new Stack<bool>();

            string[] tokens = postfixExpression.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (model.ContainsKey(token))  // token is a variable
                {
                    stack.Push(model[token]);
                }
                else  // token is an operator
                {
                    bool result;
                    switch (token)
                    {
                        case "~":
                            bool operand = stack.Pop();
                            result = !operand;
                            break;
                        case "&":
                            bool operand2 = stack.Pop(); // order of popping is reversed
                            bool operand1 = stack.Pop();
                            result = operand1 && operand2;
                            break;
                        case "=>":
                            bool b = stack.Pop(); // order of popping is reversed
                            bool a = stack.Pop();
                            result = !a || b;
                            break;
                        case "<=>":
                            bool d = stack.Pop(); // order of popping is reversed
                            bool c = stack.Pop();
                            result = (c && d) || (!c && !d);
                            break;
                        case "||":
                            bool f = stack.Pop(); // order of popping is reversed
                            bool e = stack.Pop();
                            result = e || f;
                            break;
                        default:
                            throw new ArgumentException($"Unknown operator: {token}");
                    }
                    stack.Push(result);
                }
            }
            return stack.Pop();
        }

    }
}
