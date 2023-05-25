using Inference_Engine.Models;
using Inference_Engine.Tools;
using System;
using System.Collections.Generic;
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
        }

        //[DEBUG FUNCTION]
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
            List<bool> temp_array = new List<bool>(); // Create a temporary array to store the evaluation results

            // Iterate through each sentence in the knowledge model
            foreach (string sentence in knowledgeModel.sentences)
            {
                // Evaluate the expression in the current sentence using the model and add the result to the temporary array
                temp_array.Add(evaluateExpression(sentence, model));
            }

            // Check if all the elements in the temporary array are true and return the result
            return temp_array.All(x => x);
        }


        private List<Dictionary<string, bool>> generateModels(List<string> symbols)
        {
            // Initialize the queue with an empty model.
            Queue<Dictionary<string, bool>> queue = new Queue<Dictionary<string, bool>>();

            queue.Enqueue(new Dictionary<string, bool>());

            foreach (string symbol in symbols)
            {
                // Make a temporary queue to hold the extended models.
                Queue<Dictionary<string, bool>> tempQueue = new Queue<Dictionary<string, bool>>();

                while (queue.Count > 0)
                {
                    // Remove the next model from the queue.
                    Dictionary<string, bool> model = queue.Dequeue();

                    // Extend the model with the current symbol being true and false.
                    Dictionary<string, bool> modelFalse = new Dictionary<string, bool>(model);
                    modelFalse[symbol] = false;

                    Dictionary<string, bool> modelTrue = new Dictionary<string, bool>(model);
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
            List<bool> temp_array = new List<bool>(); // Create a temporary array to store the evaluation results

            int counter = 0; // Initialize a counter variable

            // Iterate through each dictionary (model) in the list of models
            foreach (Dictionary<string, bool> m in models)
            {
                // Compare the query with the knowledge base by evaluating them using the current model
                bool KB = evaluateKnowledgeBase(m, knowledgeModel);

                bool query = evaluateExpression(knowledgeModel.query, m);

                // If the knowledge base is true, add the query result to the temporary array and increment the counter
                if (KB)
                {
                    temp_array.Add(query);
                    counter++;
                }
            }

            // Check if all the elements in the temporary array are true and store the result in a boolean variable
            bool result = temp_array.All(x => x);

            // Return a new TTQuery object with the result and the counter
            return new TTQuery(result, counter);
        }

        public bool evaluateExpression(string infixExpression, Dictionary<string, bool> model)
        {
            string postfixExpression = ShuntingYard.convertInfixToPostfix(infixExpression); // Convert the infix expression to postfix notation

            Stack<bool> stack = new Stack<bool>(); // Create a stack to store operands

            string[] tokens = postfixExpression.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries); // Split the postfix expression into tokens

            // Iterate through each token in the postfix expression
            foreach (var token in tokens)
            {
                // If the token is a variable (key in the model dictionary)
                if (model.ContainsKey(token))
                {
                    // Push the value of the variable to the stack
                    stack.Push(model[token]);
                }
                else // If the token is an operator
                {
                    // If the operator is recognized
                    if (Operators.operatorFunctions.ContainsKey(token))
                    {
                        // Pop the operands from the stack
                        bool operand2 = (token == "~" ? false : stack.Pop()); // For the unary negation operator '~', the second operand is always false
                        bool operand1 = stack.Pop();

                        // Apply the operator function to the operands and push the result to the stack
                        stack.Push(Operators.operatorFunctions[token].OperatorFunction(operand1, operand2));
                    }
                    else // If the operator is not recognized
                    {
                        // Throw an ArgumentException with an appropriate error message
                        throw new ArgumentException($"Unknown operator: {token}");
                    }
                }
            }

            // Return the result of the evaluation, which is the top value on the stack
            return stack.Pop();
        }


    }
}
