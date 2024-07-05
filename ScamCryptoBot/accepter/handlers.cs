using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ScamCryptoBot.accepter
{
    internal class handlers
    {
        public static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
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
                                    if (!DataBase.isBanned(Convert.ToInt16(chat.Id)))
                                    {
                                        await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                        string text = $"👋Добро пожаловать в CryptoFerma!🌱\n" +
                                            $"Этот бот-заявочник поможет тебе оставить заявку на вступление в наш проект.";
                                        await bot.SendTextMessageAsync(chat, text, replyMarkup: keyboard.RegisterKeys(), parseMode: ParseMode.Html);
                                    }
                                }
                                if (RegisterSteps.RunRegister) 
                                {
                                    string ticket = msg.Text;
                                    if(!string.IsNullOrEmpty(ticket)) 
                                    {
                                        string toAdm = $"🧾 Поступила новая заявка" +
                                            $"\n🧾 {ticket}\n" +
                                            $"🧾 ID: {chat.Id}\n" +
                                            $"🧾 UserName: @{chat.Username}";
                                        DataBase.AddToWaitList(Convert.ToInt32(chat.Id),chat.Username);
                                        await bot.SendTextMessageAsync(LocalConfig.adminMainId, toAdm,replyMarkup:keyboard.AcceptKeys(),parseMode:ParseMode.Html);
                                        await bot.SendTextMessageAsync(chat.Id, "🔰 Ваша заявка отправлена на рассмотрение\n" +
                                            "Я оповещу Вас, в случае одобрения.");
                                        RegisterSteps.RunRegister = false;
                                        await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                    }
                                     break;
                                }
                                if (RegisterSteps.GetTicket) 
                                {
                                    
                                    string ticket = msg.Text;
                                    if(!string.IsNullOrEmpty(ticket)) 
                                    {
                                        string[] values = ticket.Split(' ');
                                        if (values.Length == 2)
                                        {
                                            int id = Convert.ToInt32(values[0]);
                                            string userName = values[1];
                                            DataBase.RemoveFromWaitList(id, userName);
                                            DataBase.addToDataBase(id);
                                            await bot.SendTextMessageAsync(LocalConfig.adminMainId, $"☑ Готово!\n" +
                                                $"⚙ Пользователь: @{values[1]}\n" +
                                                $"⚙ ID: {values[0]}\n" +
                                                $"<b>Успешно внесен в базу данных</b>",parseMode:ParseMode.Html);
                                            await bot.SendTextMessageAsync(id, "🎉 Ваша заявка на вступление <b>одобрена!</b>🎇\n" +
                                                "И тут ссылка на бота для работы",parseMode:ParseMode.Html);
                                            RegisterSteps.GetTicket = false;
                                        }

                                    }

                                }
                                if(RegisterSteps.DenideRegister) 
                                {
                                    string ticket = msg.Text;
                                    if(!string.IsNullOrEmpty(ticket)) 
                                    {
                                        string[] values = ticket.Split(" ");
                                        int id = Convert.ToInt32(values[0]);
                                        string userName = values[1];
                                        DataBase.RemoveFromWaitList (id, userName);
                                        await bot.SendTextMessageAsync(LocalConfig.adminMainId, $"☑ Готово!\n" +
                                            $"⚙ Пользователь: @{values[1]}\n" +
                                            $"⚙ ID: {values[0]}\n" +
                                            $"<b>Успешно отказано в заявке!</b>", parseMode: ParseMode.Html);
                                        await bot.SendTextMessageAsync(id, "😞 Нам очень жаль, но мы <b>не можем принять</b> Вашу заявку!", parseMode: ParseMode.Html);
                                        RegisterSteps.GetTicket = false;
                                    }
                                }
                             
                                break;
                                
                        }
                        break;
                    // Inline keyboard actions
                    case UpdateType.CallbackQuery:
                        var callbackQuery = update.CallbackQuery;
                        switch (callbackQuery.Data)
                        {
                            // Register panel
                            case "button_reg":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id,callbackQuery.Message.MessageId);
                                string text = $"Отлично, приступим к оформлению заявки!\nВажно заполнить заявку по <b>данной форме</b>:\n" +
                                    $"1. Откуда узнали о проекте: (ваш ответ)\n" +
                                    $"2. Где работали ранее: (ваш ответ)\n" +
                                    $"3. Данный вид проекта для Вас новый или уже видели подобный: (ваш ответ)\n" +
                                    $"4. Сколько готовы уделять времени на работу: (ваш ответ)\n" +
                                    $"<b>❗ Заявки составленные не по форме будут отклонены автоматически без рассмотрения</b>\n" +
                                    $"Пришлите заявку в этот чат и ожидайте рассмотрения.";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, parseMode: ParseMode.Html);
                                RegisterSteps.RunRegister = !RegisterSteps.RunRegister;
                                break;
                            case "button_stop":
                                RegisterSteps.RunRegister = false;
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id,callbackQuery.Message.MessageId);
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Возвращайтесь, когда передумаете!");
                                break;
                            // Admin panel 
                            case "button_accpet":
                                string allWaitList = "";
                                foreach (var item in DataBase.WaitList) 
                                {
                                    allWaitList = $"Список ожидающий:\n" +
                                        $"👁‍🗨ID: <code>{item.Item1}</code>\n" +
                                        $"👁‍🗨UserName: @<code>{item.Item2}</code>";
                                }
                                text = $"{allWaitList}\n" +
                                    $"♨ Отправь мне ID и UserName пользователя:";
                                RegisterSteps.GetTicket = true;
                                await bot.SendTextMessageAsync(LocalConfig.adminMainId,text, parseMode: ParseMode.Html);

                                break;
                            case "button_denid":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                allWaitList = "";
                                foreach (var item in DataBase.WaitList) 
                                { 
                                    allWaitList = $"Список ожидающий:\n" +
                                        $"👁‍🗨ID: <code>{item.Item1}</code>\n" +
                                        $"👁‍🗨UserName: @<code>{item.Item2}</code>";
                                }
                                text = $"{allWaitList}\n" +
                                    $"♨ Отправь мне ID и UserName пользователя для отказа:";
                                await bot.SendTextMessageAsync(LocalConfig.adminMainId,text, parseMode: ParseMode.Html);
                                RegisterSteps.DenideRegister = !RegisterSteps.DenideRegister;
                                break;
                        }

                        break;

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
        public static async Task ErrorHandler(ITelegramBotClient bot, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n[{apiRequestException.Message}]",
                _ => error.ToString()
            };
        }
    }
}

