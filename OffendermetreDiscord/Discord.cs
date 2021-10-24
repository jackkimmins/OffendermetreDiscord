using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OffendermetreDiscord
{
    class Discord
    {
        //Creates an instance of the Discord socket client and the Offendermetre model.
        public DiscordSocketClient m_Client = new DiscordSocketClient();
        private OffendermetreML offendermetre = new OffendermetreML();

        //Initialise the bot and connect to Discord.
        public async Task RunAsync()
        {
            consoleText.WriteLine("Connecting to Discord...");
            await m_Client.LoginAsync(TokenType.Bot, AppConfig.DiscordBotPrivateKey);
            await m_Client.StartAsync();
            await InstallCommands();
        }

        //Initialises the event handlers and changes the bot's visible status.
        private async Task InstallCommands()
        {
            if (m_Client.LoginState != LoginState.LoggedIn) return;
            m_Client.Connected += Connected;
            m_Client.MessageReceived += MessageReceived;

            //If the bot is running on a so-called "development machine", display a predefined status message.
            if (Environment.MachineName == AppConfig.DevMachineName)
                await m_Client.SetGameAsync("Development Mode", "https://www.twitch.tv/this-is-just-a-blank-link", ActivityType.Streaming);
            else
                await m_Client.SetStatusAsync(UserStatus.DoNotDisturb);
        }

        //Outputs a message to the console when connected to Discord.
        private Task Connected()
        {
            consoleText.WriteLine("Connected to Discord!\r\n");
            return Task.CompletedTask;
        }

        //Fires when the bot receives a message.
        private async Task MessageReceived(SocketMessage messageParam)
        {
            SocketUserMessage message = messageParam as SocketUserMessage;
            if (message == null) return;

            CommandContext context = new CommandContext(m_Client, message);
            
            //Prevent the bot from replying to itself.
            if (message.Author.Id != m_Client.CurrentUser.Id)
            {
                //Checks to see if the message was sent from a channel in a Discord server.
                if (message.Channel is SocketGuildChannel)
                {
                    //Queries the given input against the Offendermetre model.
                    SentimentPrediction prediction = offendermetre.Query(message.Content);

                    // True = Toxic | False = Non-toxic
                    if (prediction.Prediction)
                    {
                        //Deletes the toxic Discord message.
                        await context.Channel.DeleteMessageAsync(message.Id);

                        //Posts a message in the current channel stating that message was blocked.
                        await context.Channel.SendMessageAsync("**" + context.Message.Author.Username + "'s Message was blocked!**\nMessage was identified as being offensive.\n> " + prediction.Probability * 100 + "%\n");

                        //Sends a message to the individual user, saying that their message was blocked.
                        await m_Client.GetUser(message.Author.Id).SendMessageAsync($"**[Profanity Alert]**\nYour message sent in {context.Channel.Name} channel on **{context.Guild.Name}** could be considered offensive.\nMessage: `{message.Content}`\nPlease reword your original message and send it again.");
                    }
                    else
                    {
                        //React to a non-toxic message with the Offendermetre tick emoji.
                        await context.Message.AddReactionAsync(m_Client.GetGuild(525411204756406282).Emotes.First(e => e.Name == "OffendermetreTick"));
                    }

                    //Logs to the console.
                    consoleText.WriteLine($"<{message.Author.ToString()} in {context.Guild.Name}> {message.ToString()}", "MSG-" + (prediction.Prediction ? "Block" : "Safe"), prediction.Prediction ? ConsoleColor.Red : ConsoleColor.Green);
                }
                else
                {
                    //Outputs messages directly send the bot to the console.
                    consoleText.WriteLine($"<{message.Author.ToString()}> {message.Content}");
                }
            }
        }
    }
}