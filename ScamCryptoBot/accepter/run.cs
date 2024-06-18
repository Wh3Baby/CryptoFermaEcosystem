﻿using Telegram.Bot.Polling;
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
            tokenBot = "6757839183:AAF_3C6QzpoYJ1Yws1tgOLKsn8w5a1PEhEM";
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

