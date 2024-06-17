using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace ScamCryptoBot.accepter
{
    internal class run
    {
        public static ITelegramBotClient telegramClient;
        private static ReceiverOptions receiverOptions;
        private static string tokenBot;
        private static CancellationTokenSource cancellationTokenSource;

        public static async Task RunAccepter(CancellationToken cancellationToken)
        {
            tokenBot = "7206594455:AAHjs4c8GPweT7khOpEFxqQgh6TxbM4c8X4";
            telegramClient = new TelegramBotClient(tokenBot);
            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
                ThrowPendingUpdates = true,
            };

            using (cancellationTokenSource = new CancellationTokenSource())
            {
                telegramClient.StartReceiving(handlers.UpdateHandler,handlers.ErrorHandler, receiverOptions, cancellationTokenSource.Token);
                await Task.Delay(TimeSpan.FromDays(999), cancellationToken);

                // Проверить статус отмены
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}

