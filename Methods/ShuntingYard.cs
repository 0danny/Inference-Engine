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
            List<string> tokens = new();

            infix = infix.Replace(" ", "");

            string buffer = "";

            for(int i = 0; i < infix.Length; i++)
            {
                if (infix[i] != ' ')
                {
                    //Loop through each operator symbol and check for them.
                    bool usedSplit = false;

                    foreach(string operatorValue in Operators.operatorsSplit)
                    {
                        if (infix[i] == operatorValue[0])
                        {
                            tokens.Add(operatorValue);

                            i += (operatorValue.Length > 1 ? operatorValue.Length - 1 : 0);

                            usedSplit = true;

                            break;
                        }
                    }

                    if(usedSplit)
                    {
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

        public static string convertInfixToPostfix(string infix)
        {
            Stack<string> stack = new Stack<string>();

            Queue<string> outputQueue = new Queue<string>();

            List<string> tokens = tokenParser(infix);

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (Operators.operatorFunctions.ContainsKey(token))
                {
                    while (stack.Count > 0 && Operators.operatorFunctions[token].OperatorPriority <= (Operators.operatorFunctions.ContainsKey(stack.Peek()) ? Operators.operatorFunctions[stack.Peek()].OperatorPriority : -1))
                    {
                        outputQueue.Enqueue(stack.Pop());
                    }

                    stack.Push(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        outputQueue.Enqueue(stack.Pop());
                    }

                    if (stack.Count > 0 && stack.Peek() == "(")
                    {
                        stack.Pop();
                    }
                }
                else // Operand
                {
                    outputQueue.Enqueue(token);
                }
            }

            while (stack.Count > 0)
            {
                outputQueue.Enqueue(stack.Pop());
            }

            return string.Join(" ", outputQueue);
        }
    }
}

