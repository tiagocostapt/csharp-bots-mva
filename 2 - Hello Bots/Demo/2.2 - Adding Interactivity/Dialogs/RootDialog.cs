using Microsoft.Bot.Builder.Dialogs;
using System;
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

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Text(context, NameEntered, @"Hi! What's your name?");

            return Task.CompletedTask;
        }

        private async Task NameEntered(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync($@"Hi {await result}!");

            context.Wait(MessageReceivedAsync);
        }
    }
}