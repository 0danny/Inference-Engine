using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Tools
{
    public class SearchUtilities
    {
        public static List<Tuple<int, int>>? findBrackets(string str)
        {
            Stack<int> stack = new Stack<int>();

            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(')
                {
                    stack.Push(i);
                }
                else if (str[i] == ')')
                {
                    if (stack.Count > 0)
                    {
                        pairs.Add(new Tuple<int, int>(stack.Pop(), i));
                    }
                    else
                    {
                        Console.WriteLine("[STR] Unbalanced brackets found.");
                        return null;
                    }
                }
            }

            if (stack.Count > 0)
            {
                Console.WriteLine("[STACK] Unbalanced brackets found.");
                return null;
            }

            return pairs;
        }
    }
}

