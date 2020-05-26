using System;
using System.Threading.Tasks;
using Discord.Commands;


namespace mr.Modules
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
