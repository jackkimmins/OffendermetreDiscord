using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//https://discord.com/api/oauth2/authorize?client_id=637270199753768990&permissions=76864&scope=bot

namespace OffendermetreDiscord
{
    //Responsible for the configuration of this Offendermetre instance.
    class AppConfig
    {
        public static string DiscordBotPrivateKey { get; set; }
        public static ulong AdminDiscordID { get; set; }
        public static string DevMachineName { get; set; }
        public static bool ShouldTickSafeMsgs { get; set; } = false;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Populates the configuration for this Offendermetre instance.
            AppConfig.DiscordBotPrivateKey = "KEY";
            AppConfig.AdminDiscordID = 123;
            AppConfig.DevMachineName = "PC_NAME";

            Console.Title = "Offendermetre Discord";
            consoleText.welcomeText();
            consoleText.WriteLine("Starting Offendermetre Discord server...");

            //Creates a new instance of the Discord bot.
            Discord discord = new Discord();
            System.Threading.Tasks.Task.Run(() => discord.RunAsync());

            //Gets the commands entered on this local console program.
            while (true)
            {
                string rLine = Console.ReadLine();

                if (rLine == "stop")
                {
                    Environment.Exit(0);
                }
                else
                {
                    consoleText.WriteLine("Bot Status: " + discord.m_Client.CurrentUser.Status);
                }
            }
        }
    }
}
