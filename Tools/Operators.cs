using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Tools
{
    public class Operators
    {
        //This class contains every single operator and operator list needed to preform each one of the methods.

        //Used for shunting yard token parser.
        public static string[] operatorsSplit = new string[]
        {
            "(",
            ")",
            "<=>",
            "=>",
            "||",
            "~",
            "&"
        };

        //Used for shunting yard token parser.
        public static bool checkContainsSplit(char charCheck)
        {
            return operatorsSplit.Any(c => c[0] == charCheck);
        }

        //Used for shunting yard infix to prefix conversion and for evaluating.
        public static Dictionary<string, int> operatorsPrecedence = new Dictionary<string, int>()
        {
            ["~"] = 4,
            ["&"] = 3,
            ["||"] = 2,
            ["=>"] = 1,
            ["<=>"] = 0
        };
    }
}
