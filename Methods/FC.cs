using Inference_Engine.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

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
            FCQuery entails = checkEntailment(model);

            Console.WriteLine(entails.getEntailmentResponse());
        }

        private FCQuery checkEntailment(KnowledgeModel model)
        {
            bool result = false;

            //find all rules and create list
            List<string> rules = new List<string>();
            rules = model.sentences.Where(s => s.Contains("=>")).ToList();

            //find all facts and add to the queue
            List<string> queue = new List<string>();
            queue = model.sentences.Where(s => !s.Contains("=>")).ToList();

            //create a dictionary which links rules with their corresponding values
            IDictionary<string, int> values = new Dictionary<string, int>();

            foreach (string s in rules)
            {
                if (s.Contains("&"))
                {
                    values.Add(s, 2);
                }
                else
                {
                    values.Add(s, 1);
                }
            }

            //loop through the queue, if queue becomes empty then return false
            int i = 0;
            while (i < queue.Count)
            {
                //look at the symbol next in line in queue and find all rules where it is present before the '=>'
                string symbol = queue[i];
                List<string> filteredSentences = rules.Where(s => s.Contains(symbol) && s.IndexOf(symbol) < s.IndexOf("=>")).ToList();
                
                //if the query is found return true
                if (filteredSentences.Any(s => s.Contains(model.query)))
                {
                    result = true; break;
                }

                //otherwise if there are rules to test, test them
                if (filteredSentences.Count != 0)
                {
                    evaluateSentences(values, filteredSentences, queue);
                }

                i++;
            }

            return new FCQuery(result, queue);
        }

        private List<string> evaluateSentences(IDictionary<string, int> values, List<string> filteredSentences, List<string> queue)
        {
            //look at each rule and determine if it can be added to queue
            int i = 0;
            while (i < filteredSentences.Count)
            {
                int tempvalue = values[filteredSentences[i]] - 1;
                
                //if the value of the rule equals 0, remove from the dictionary and add the symbol found after the '=>' to the queue
                if (tempvalue == 0)
                {
                    values.Remove(filteredSentences[i]);
                    queue.Add(filteredSentences[i].Split(new[] { "=>" }, StringSplitOptions.None)[1].Trim());
                }
                
                i++;
            }

            return queue;
        }
    }
}
