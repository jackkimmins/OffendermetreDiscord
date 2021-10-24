using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffendermetreDiscord
{
    //Responsible for console outputs.
    class consoleText
    {
        //Displays the welcome text.
        public static void welcomeText()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Offendermetre Discord Client [Version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}]");
            Console.WriteLine("(c) " + DateTime.Now.Year.ToString() + " Stable Network. All rights reserved.");
            Console.WriteLine();
            Console.ResetColor();
        }

        //Displays an output on the console prefixed with the current time and thread that is being executed on.
        public static void WriteLine(string value, string threadName = "INFO", ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            string outputPrefix = $"[{DateTime.Now.ToString("HH:mm:ss")}] [Thread/{threadName}]: ";

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(outputPrefix);
            Console.ResetColor();

            Console.ForegroundColor = consoleColor;
            Console.WriteLine(value);
            Console.ResetColor();

            Log.WriteLogLine(outputPrefix + value);
        }
    }
}
