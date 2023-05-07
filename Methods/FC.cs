using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Methods
{
    public class FC : Method
    {
        public FC()
        {
            name = "Forward Chaining";
            shortName = "FC";
        }

        public override void runMethod(KnowledgeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
