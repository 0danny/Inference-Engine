using Inference_Engine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inference_Engine.Methods
{
    public class ShuntingYard
    {
        public static List<string> tokenParser(string infix)
        {
            //Initialize new set of tokens.
            List<string> tokens = new List<string>();

            //If there is any spaces in the expression remove them.
            infix = infix.Replace(" ", "");

            //Buffer for symbols bigger than 1 character
            string buffer = "";

            //Loop through all of the characters in the expression
            for (int i = 0; i < infix.Length; i++)
            {
                //If the character is valid
                if (infix[i] != ' ')
                {
                    //Loop through each operator symbol and check for them.
                    bool usedSplit = false;

                    //Check if the current character is the same as the first character of any operator.
                    foreach (string operatorValue in Operators.operatorsSplit)
                    {
                        if (infix[i] == operatorValue[0])
                        {
                            //Add it is a token
                            tokens.Add(operatorValue);

                            //Move the loop along (size of the operator length)
                            i += (operatorValue.Length > 1 ? operatorValue.Length - 1 : 0);

                            usedSplit = true;

                            break;
                        }
                    }

                    if (usedSplit)
                    {
                        //Go back through the loop if we have modified the iterator.
                        continue;
                    }

                    //We have found a symbol character.
                    buffer += infix[i];

                    if ((i == infix.Length - 1) ? true : Operators.checkContainsSplit(infix[i + 1]))
                    {
                        //Buffer is completed add to list.
                        tokens.Add(buffer);

                        //Reset buffer.
                        buffer = "";
                    }
                    else
                    {
                        //There is still more characters to read.
                        continue;
                    }
                }
            }

            return tokens;
        }

        //GeneralKB3 =  "a c d ~ & <=> b & b a => &".

        // This function converts an infix expression to a postfix expression.
        public static string convertInfixToPostfix(string infix)
        {
            Stack<string> stack = new Stack<string>(); // Stack to store operators

            Queue<string> outputQueue = new Queue<string>(); // Queue to store output (postfix) tokens

            List<string> tokens = tokenParser(infix); // Parse the infix expression into a list of tokens

            // Iterate through the tokens
            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i]; // Get the current token

                // If the token is an operator
                if (Operators.operatorFunctions.ContainsKey(token))
                {
                    // Pop operators from the stack and enqueue them to the output queue
                    // until an operator with lower precedence is encountered
                    while (stack.Count > 0 && Operators.operatorFunctions[token].OperatorPriority <= (Operators.operatorFunctions.ContainsKey(stack.Peek()) ? Operators.operatorFunctions[stack.Peek()].OperatorPriority : -1))
                    {
                        outputQueue.Enqueue(stack.Pop());
                    }

                    // Push the current operator to the stack
                    stack.Push(token);
                }
                // If the token is an opening parenthesis
                else if (token == "(")
                {
                    // Push the opening parenthesis to the stack
                    stack.Push(token);
                }
                // If the token is a closing parenthesis
                else if (token == ")")
                {
                    // Pop operators from the stack and enqueue them to the output queue
                    // until an opening parenthesis is encountered
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        outputQueue.Enqueue(stack.Pop());
                    }

                    // Remove the opening parenthesis from the stack
                    if (stack.Count > 0 && stack.Peek() == "(")
                    {
                        stack.Pop();
                    }
                }
                else // Operand
                {
                    // Enqueue the operand to the output queue
                    outputQueue.Enqueue(token);
                }
            }

            // Pop any remaining operators from the stack and enqueue them to the output queue
            while (stack.Count > 0)
            {
                outputQueue.Enqueue(stack.Pop());
            }

            // Convert the output queue to a string representation and return it
            return string.Join(" ", outputQueue);
        }
    }
}

