using ScamCryptoBot.notcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ScamCryptoBot.worker
{
    internal class handlers
    {
        public static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:

                        var msg = update.Message;
                        var user = msg.From;
                        var chat = msg.Chat;
                        switch (msg.Type)
                        {
                            // Command actions
                            case MessageType.Text:
                                if (msg.Text == "/start") 
                                { 
                                   if(DataBase.IsWorker(Convert.ToInt32(chat.Id))) 
                                    {
                                        string welcome = $"Здарова, миллионер! 💰\n" +
                                            $"🇱 Cейчас у тебя: профитов\n" +
                                            $"🇱 На сумму: RUB\n" +
                                            $"🇱 Звание: \n" +
                                            $"🇱 Твой ID: <code>{chat.Id}</code>\n" +
                                            $"🇱 Твой код привязки мамонта: \n" +
                                            $"🇱 Твой процент с залёта: {LocalConfig.PercentOfWorkers}%";
                                        await bot.SendTextMessageAsync(chat.Id, welcome, parseMode: ParseMode.Html);
                                    }
                                    else 
                                    {
                                        string noWelcome = $"{user.FirstName}, нам жаль, но данный бот только для воркеров проекта!\n" +
                                            $"Если хочешь стать миллионером, но не можешь встать с кровати\n" +
                                            $"То оставляй заявку в нашем боте : @crypto_ferma_acc_bot";
                                        await bot.SendTextMessageAsync(chat.Id,noWelcome, parseMode: ParseMode.Html);
                                    }
                                }

                                break;
                        }
                     break ;
                    // Inline keyboard actions
                    case UpdateType.CallbackQuery:
                        var callbackQuery = update.CallbackQuery;
                        switch (callbackQuery.Data)
                        {
                            
                        }

                        break;

                }
            }
            catch (Exception ex)
            {


            }
        }
        public static async Task ErrorHandler(ITelegramBotClient bot, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[]",
                _ => error.ToString()
            };
        }
    }
}

