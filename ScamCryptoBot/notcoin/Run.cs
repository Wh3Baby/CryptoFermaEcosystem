
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ScamCryptoBot.notcoin
{
    internal class Run
    {
        private static ITelegramBotClient telegramClient;
        private static ReceiverOptions receiverOptions;
        private static string tokenBot;
        private static CancellationTokenSource cancellationTokenSource; 

        public static async Task RunNotcoinScamBot(CancellationToken cancellationToken)
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
                telegramClient.StartReceiving(Handlers.UpdateHandler, Handlers.ErrorHandler, receiverOptions, cancellationTokenSource.Token);
                await Task.Delay(TimeSpan.FromDays(999),cancellationToken); 

                // Проверить статус отмены
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }


}
    

