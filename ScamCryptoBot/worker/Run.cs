using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace ScamCryptoBot.worker
{
    internal class Run
    {
        public static ITelegramBotClient telegramClient;
        private static ReceiverOptions receiverOptions;
        private static string tokenBot;
        private static CancellationTokenSource cancellationTokenSource;

        public static async Task RunWorkerBot(CancellationToken cancellationToken)
        {
            
            tokenBot = "7421631557:AAHRHZjyYlWsh_fGf460DIYIfIGQcWl-_w4";
            telegramClient = new TelegramBotClient(tokenBot);
            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
                ThrowPendingUpdates = true,
            };

            using (cancellationTokenSource = new CancellationTokenSource())
            {
                telegramClient.StartReceiving(handlers.UpdateHandler,handlers.ErrorHandler,receiverOptions, cancellationTokenSource.Token);
                await Task.Delay(TimeSpan.FromDays(999), cancellationToken);

                // Проверить статус отмены
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}

