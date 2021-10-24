using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffendermetreDiscord
{
    class Log
    {
        //Writes the provided line to the log file.
        public static Task WriteLogLine(string line)
        {
            System.IO.File.AppendAllText("OffendermetreLog.txt", line + Environment.NewLine);
            return Task.CompletedTask;
        }
    }
}
