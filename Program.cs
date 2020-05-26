using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace mr
{
    public class DiscordBot
    {
        DiscordSocketClient client;
        CommandService commands;
        IServiceProvider services;

        Settings settings;
        string prefix;

        public static void Main(string[] args) => new DiscordBot().Run().GetAwaiter().GetResult();

        public async Task Run()
        {
            settings = new Settings();

            client = new DiscordSocketClient();
            commands = new CommandService();

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();

            string token = settings.getToken();
            prefix = settings.getPrefix();

            client.Log += Client_Log;

            await Commands();
            await client.LoginAsync(TokenType.Bot,token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task Commands()
        {
            client.MessageReceived += CommandHandler;
            await commands.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), services);
        }

        async Task CommandHandler(SocketMessage message)
        {
            var msg = message as SocketUserMessage;
            var commandContext = new SocketCommandContext(client,msg);

            if (message.Author.IsBot) return;

            int argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(commandContext, argPos, services);

                if (!result.IsSuccess) {
                    Console.WriteLine("there was an error: " + result.ErrorReason);
                }
            }
        }
    }
}
