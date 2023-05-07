using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }
    }
}
