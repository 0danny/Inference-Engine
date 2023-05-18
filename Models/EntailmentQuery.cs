using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inference_Engine.Models
{
    public class TTQuery
    {
        private bool response { get; set; } = false;
        private int symbolsEntailed { get; set; } = 0;

        public TTQuery(bool response, int symbolsEntailed)
        {
            this.response = response;
            this.symbolsEntailed = symbolsEntailed;
        }

        public string getEntailmentResponse()
        {
            if(symbolsEntailed == 0)
            {
                return "NO";
            }

            return response ? $"YES: {symbolsEntailed}" : "NO";
        }
    }

    public class FCQuery
    {
        private bool response { get; set; } = false;
        private List<string> queue { get; set; } = new();

        public FCQuery(bool response, List<string> queue)
        {
            this.response = response;
            this.queue = queue;
        }

        public string getEntailmentResponse()
        {
            return response ? $"YES: [{String.Join(",", queue)}]" : "NO";
        }
    }

    public class BCQuery
    {
        private bool response { get; set; } = false;
        private List<string> searchpath { get; set; } = new();

        public BCQuery(bool response, List<string> searchpath)
        {
            this.response = response;
            this.searchpath = searchpath;
        }

        public string getEntailmentResponse()
        {
            return response ? $"YES: [{String.Join(",", searchpath)}]" : "NO";
        }
    }
}
