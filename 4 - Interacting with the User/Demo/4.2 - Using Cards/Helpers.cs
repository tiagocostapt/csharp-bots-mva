using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application
{
    internal static class Helpers
    {
        public static async Task<ThumbnailCard> CreateCardForUser(string username)
        {
            var profile = await GitHubClient.LoadProfile(username);
            var card = new ThumbnailCard
            {
                Images = new[] { new CardImage(profile.AvatarUrl) }
            };

            if (!string.IsNullOrWhiteSpace(profile.Name)) card.Subtitle = profile.Name;

            string text = string.Empty;
            if (!string.IsNullOrWhiteSpace(profile.Company)) text += profile.Company + " \n";
            if (!string.IsNullOrWhiteSpace(profile.Email)) text += profile.Email + " \n";
            if (!string.IsNullOrWhiteSpace(profile.Bio)) text += profile.Bio;
            card.Text = text;

            card.Tap = new CardAction(ActionTypes.OpenUrl, profile.HtmlUrl);
            return card;
        }
    }
}