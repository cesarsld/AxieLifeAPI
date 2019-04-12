using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using AxieLifeAPI.Models.SingleElimination;
using AxieLifeAPI.IO;
using AxieLifeAPI.Models.User;

namespace AxieLifeAPI.Models.Notification
{
    public class NotificationSender
    {
        private static DiscordRestClient restClient;

        public async static Task StartClient()
        {
            if (restClient == null)
                restClient = new DiscordRestClient(new DiscordRestConfig
                {
                    LogLevel = LogSeverity.Verbose,
                });

            await restClient.LoginAsync(TokenType.Bot, IOGetter.GetDiscordKey());
        }

        public static async Task SendMessage(ParticipantData participant, string message)
        {
            if (participant.IsDiscordIdValid())
            {
                var user = await restClient.GetUserAsync(participant.discordId);
                await user.SendMessageAsync(message);
            }
        }

        public static async Task SendMessage(UserData userData, string message)
        {
            if (userData.IsDiscordIdValid())
            {
                var user = await restClient.GetUserAsync(userData.discordId);
                await user.SendMessageAsync(message);
            }
        }

    }
}
