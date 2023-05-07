using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Models
{
    public abstract class Method
    {
        public string name { get; set; } = "Unknown";
        public string shortName { get; set; } = "Unknown";

        public abstract void runMethod(KnowledgeModel model);
    }
}
