using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public static Dictionary<string, OperatorBase> operatorFunctions = new Dictionary<string, OperatorBase>()
        {
            ["~"] = new OperatorBase(4, (x, y) => !x),
            ["&"] = new OperatorBase(3, (x, y) => x && y),
            ["=>"] = new OperatorBase(1, (x, y) => !x || y),
            ["<=>"] = new OperatorBase(0, (x, y) => (x && y) || (!x && !y)),
            ["||"] = new OperatorBase(2, (x, y) => x || y)
        };
    }

    public class OperatorBase
    {
        public int OperatorPriority { get; set; }
        public Func<bool, bool, bool> OperatorFunction { get; set; }

        public OperatorBase(int operatorPriority, Func<bool, bool, bool> operatorFunction)
        {
            OperatorPriority = operatorPriority;
            OperatorFunction = operatorFunction;
        }
    }
}


