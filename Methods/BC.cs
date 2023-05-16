using Inference_Engine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
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
            EntailmentQuery entails = checkEntailment(model);

            Console.WriteLine(entails.getEntailmentResponse());
        }

        private EntailmentQuery checkEntailment(KnowledgeModel model)
        {
            //find all rules and create list
            List<string> rules = new List<string>();
            rules = model.sentences.Where(s => s.Contains("=>")).ToList();

            //find all facts and create list
            List<string> facts = new List<string>();
            facts = model.sentences.Where(s => !s.Contains("=>")).ToList();

            //create list to hold searchpath
            List<string> searchpath = new List<string>();
            searchpath.Add(model.query);

            return new EntailmentQuery(evaluateSentences(model.query, rules, facts, searchpath), searchpath);
        }

        private bool evaluateSentences(string symbol, List<string> rules, List<string> facts, List<string> searchpath)
        {
            List<string> filteredSentences = rules.Where(s => s.Contains(symbol) && s.IndexOf(symbol) > s.IndexOf("=>")).ToList();
            
            foreach (string s in filteredSentences)
            {
                string news = s.Split(new[] { "=>" }, StringSplitOptions.None)[0].Trim();

                if (news.Contains("&"))
                {
                    string[] parts = news.Split(new[] { "&" }, StringSplitOptions.None);
                    bool[] partstruth = new bool[parts.Length];

                    int i = 0;
                    while(i < parts.Length)
                    {
                        if (isFact(parts[i], rules))
                        {
                            partstruth[i] = true;
                        }
                        else
                        {
                            partstruth[i] = evaluateSentences(parts[i], rules, facts, searchpath);
                        }
                        i++;
                    }

                    if(partstruth.All(b => b == true))
                    {
                        return true;
                    }

                    else
                    {
                        return false;
                    }
                }

                else
                {
                    if (isFact(news, facts))
                    {
                        searchpath.Add(news);
                        return true;
                    }
                    else
                    {
                        searchpath.Add(news);
                        return evaluateSentences(news, rules, facts, searchpath);
                    }
                }
            }

            return false;
        }

        private bool isFact(string s, List<string> facts)
        {
            if (facts.Contains(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private class EntailmentQuery
        {
            private bool response { get; set; } = false;
            private List<string> searchpath { get; set; } = null;

            public EntailmentQuery(bool response, List<string> searchpath)
            {
                this.response = response;
                this.searchpath = searchpath;
            }

            public string getEntailmentResponse()
            {
                if (!response)
                {
                    return "NO";
                }
                else
                {
                    return $"YES: [{String.Join(",", searchpath)}]";
                }
            }
        }
    }
}
