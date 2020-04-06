using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Threading.Tasks.Dataflow;

namespace Questes2
{
    class Program
    {

        static void Main(string[] args)
        {
            Host host = new Host();
            host.Run();
            Console.ReadKey();
        }
    }

    internal class Host
    {
        [ImportMany(typeof(ILogger))]
        protected IEnumerable<ILogger> _tabs = null;

        public void Run()
        {
            var container = new CompositionContainer();
            container.ComposeParts(this, new OutcomeStandart(), new OutcomeFileTxt());

            foreach (var tab in _tabs)
            {
                tab.Write("Hello !");
            }
        }
    }
    internal interface ILogger
    {
        void Write(string message);
    }

    [Export(typeof(ILogger))]
    internal class OutcomeStandart : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    [Export(typeof(ILogger))]
    internal class OutcomeFileTxt : ILogger
    {
        public void Write(string message)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "text.txt";

            if (!File.Exists(filepath))
            {
                var file = File.Create(filepath);
                file.Close();
            }
                
            TextWriter tw = new StreamWriter(filepath, true);
            tw.WriteLine(message, true);
            tw.Close();
            Console.WriteLine($"Message has just added to your file on  {filepath} \nPlease check it !");
        }
    }
}
