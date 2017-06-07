using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot_Application.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var msg = await result as IMessageActivity;
            if (msg.Text.Equals(@"search", StringComparison.OrdinalIgnoreCase))
            {
                PromptDialog.Text(context, QueryEntered, @"Who do you want to search for?");
            }
            else if (msg.Text.StartsWith(@"search ", StringComparison.OrdinalIgnoreCase))
            {
                var query = msg.Text.Substring(7);
                await context.Forward<string, string>(new SearchDialog(), SearchComplete, query, default(CancellationToken));
            }
        }

        private async Task QueryEntered(IDialogContext context, IAwaitable<string> result)
        {
            await context.Forward<string, string>(new SearchDialog(), SearchComplete, await result, default(CancellationToken));
        }

        private async Task SearchComplete(IDialogContext context, IAwaitable<string> result)
        {
            var returnMessage = await result;
            if (!string.IsNullOrWhiteSpace(returnMessage)) await context.PostAsync(returnMessage);

            context.Wait(MessageReceivedAsync);
        }

    }
}