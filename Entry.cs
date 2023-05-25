using Inference_Engine.Methods;
using Inference_Engine.Models;
using Inference_Engine.Tools;
using System;
using System.IO;
using System.Linq;

namespace Inference_Engine
{
    public class Entry
    {
        private Method[] methodList = new Method[]
        {
            new BC(), //Backward Chaining
            new FC(), //Forward Chaining
            new TT() //Truth Table Checking
        };

        private FileParser parser = new FileParser();

        public Entry(string[] args)
        {
            //Ensure arguments are available
            if(args.Length <= 1)
            {
                Console.WriteLine("There were no/not enough arguments inputted into the program.");
                Console.WriteLine("iengine.exe [METHOD] [FILENAME]");

                return;
            }

            //Ensure the method exists.
            Method methodSearch = methodList.FirstOrDefault(elem => elem.shortName == args[0]);

            if(methodSearch != null)
            {
                //Ensure the file exists.
                if (File.Exists(args[1]))
                {
                    //Parse the file using Knowledge Model base.
                    KnowledgeModel model = parser.parseFile(args[1]);

                    //Send the Knowledge Model to the appropriate search method.
                    methodSearch.runMethod(model);
                }
                else
                {
                    Console.WriteLine($"The file path [{args[1]}] does not exist.");
                }
            }
            else
            {
                //If method does not exist, explain and then print all of the available methods.
                Console.WriteLine($"Could not find method: [{args[0]}], available methods are:");

                //List out all of the methods.
                Console.WriteLine(string.Join(Environment.NewLine, methodList.Select(m => $"[{m.shortName}] {m.name}")));
            }

            Console.ReadLine();
        }

        private static void Main(string[] args)
        {
            Entry entry = new Entry(args);
        }
    }
}