using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Methods
{
    public class BC : Method
    {
        public BC()
        {
            name = "Backward Chaining";
            shortName = "BC";
        }

        public override void runMethod(KnowledgeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
