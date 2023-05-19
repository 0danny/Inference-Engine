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

            //initialise search list
            List<string> searchlist = new List<string>();
            searchlist = model.sentences.Where(s => !s.Contains("=>")).ToList();


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
            while (queue.Count != 0)
            {
                //look at the symbol next in line in queue and find all rules where it is present before the '=>'
                string symbol = queue.First();
                List<string> filteredSentences = rules.Where(s => s.Contains(symbol) && s.IndexOf(symbol) < s.IndexOf("=>")).ToList();

                //look at each dictionary value, if it includes the query and its value is 0, return true
                foreach (var v in values)
                {
                    if (v.Key.Contains(model.query) && v.Value == 0)
                    {
                        result = true;
                        return new FCQuery(result, searchlist);
                    }
                }                

                //otherwise if there are rules to test, test them
                if (filteredSentences.Count != 0)
                {
                    evaluateSentences(values, filteredSentences, queue, searchlist);
                }
                queue.Remove(symbol);
            }

            return new FCQuery(result, queue);
        }

        private List<string> evaluateSentences(IDictionary<string, int> values, List<string> filteredSentences, List<string> queue, List<string> searchlist)
        {
            //look at each rule and determine if it can be added to queue
            int i = 0;
            while (i < filteredSentences.Count)
            {
                //lower the value by 1
                values[filteredSentences[i]] = values[filteredSentences[i]] - 1;

                //if the value of the rule equals 0, remove from the dictionary and add the symbol found after the '=>' to the queue
                if ((values[filteredSentences[i]]) == 0)
                {
                    queue.Add(filteredSentences[i].Split(new[] { "=>" }, StringSplitOptions.None)[1].Trim());
                    searchlist.Add(filteredSentences[i].Split(new[] { "=>" }, StringSplitOptions.None)[1].Trim());
                }
                
                i++;
            }

            return queue;
        }
    }
}
