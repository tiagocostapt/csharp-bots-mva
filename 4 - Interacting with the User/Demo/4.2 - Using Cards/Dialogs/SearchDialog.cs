using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Application.Dialogs
{
    [Serializable]
    internal class SearchDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait<string>(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var query = await result;

            var profiles = await GitHubClient.ExecuteSearch(query);

            var totalCount = profiles.TotalCount;

            if (totalCount == 0)
            {
                context.Done(@"Sorry, no results found.");
            }
            else if (totalCount > 10)
            {
                context.Done(@"More than 10 results were found. Please narrow your search");
            }
            else
            {
                // convert the results in to an array of cards for each user
                var userCards = profiles.Items.Select(item => CreateCard(item));

                var msg = context.MakeMessage();
                msg.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                msg.Attachments = userCards.ToList();

                await context.PostAsync(msg);
                context.Done(default(string));
            }
        }

        private static Attachment CreateCard(Octokit.User profile) =>
            new ThumbnailCard()
            {
                Title = profile.Login,
                Images = new[] { new CardImage(url: profile.AvatarUrl) },
                Buttons = new[] { new CardAction(ActionTypes.OpenUrl, @"Click to view", value: profile.HtmlUrl) }
            }.ToAttachment();
    }
}