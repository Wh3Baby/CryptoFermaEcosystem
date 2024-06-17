using ScamCryptoBot.notcoin;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ScamCryptoBot
{
    internal class MainHandlers
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
                                if (msg.Text == "/start" && chat.Id == LocalConfig.adminMainId)
                                {
                                    await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                    string text = $"❤️‍🔥 {chat.FirstName}, с возвращением!\n" +
                                        $"🤖 Я - главный бот управления всей экосистемой 🌳\n" +
                                        $"⚙️ Разработчик: @WH3BABY\n" +
                                        $"👇 Клавиатура ниже в твоем распоряжении";
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, text,replyMarkup:MainKeyBoard.MainMenu(),parseMode:ParseMode.Html);
                                    break;
                                }
                                
                                break;
                        }
                        break;
                    // Inline keyboard actions
                    case UpdateType.CallbackQuery:
                        var callbackQuery = update.CallbackQuery;
                        switch (callbackQuery.Data)
                        {
                            // Main Menu after command /start
                            case "mbtn_configuration":
                                // Config Panel
                                string text = "⚙️ Вы вошли в меню конфигурации всей экосистемы 🌳\n" +
                                    "Будьте осторожнее с настройками.";
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId,text,replyMarkup:MainKeyBoard.ConfigPanel(),parseMode:ParseMode.Html);
                                }
                                break;
                                // Admin panel
                            case "mbtn_admin_panel":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = "⚙️ Вы вошли в админ панель\nВерсия: 0.1";
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, text, replyMarkup: MainKeyBoard.AdminPanel(), parseMode: ParseMode.Html);
                                }
                                break;
                            // Buttons in APanel
                            case "mbtn_main_backup_database":
                                await DataBase.DataBaseRecover();
                                break;
                                // BotPanel
                            case "mbtn_bots_panel":
                                if(LocalConfig.adminMainId == callbackQuery.Message.Chat.Id) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = "⚙️ Вы вошли в панель управления ботами всей экосистемы 🌳\n";
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId,text,replyMarkup: MainKeyBoard.BotsPanel(),parseMode: ParseMode.Html);
                                }
                                break;
                                
                            // BOTS PANEL_NOTCOIN
                            case "mbtn_notcoin":
                                
                                // double check ID 
                                if (callbackQuery.Message.Chat.Id == LocalConfig.adminMainId) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                     string sstatusBot = "Остановлен";
                                    if (LocalConfig.isNotcoinRun)
                                        sstatusBot = "Запущен";
                                    text = $"🔐 Статус бота: {sstatusBot}";
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, text, replyMarkup: MainKeyBoard.BotsPanelNotcoin(),parseMode:ParseMode.Html);
                                }
                                break;
                            // start stop Notcoin bot
                            case "mbtn_bots_notcoin_status":
                                if (LocalConfig.isNotcoinRun == false)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isNotcoinRun = true;
                                    Task.Run(() => notcoin.Run.RunNotcoinScamBot(cancellationToken));
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, "Бот успешно запущен", replyMarkup: MainKeyBoard.BotsPanelNotcoin());
                                }
                                if(LocalConfig.isNotcoinRun == true)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isNotcoinRun = false;
                                    cancellationTokenSource.Cancel(); // Остановить таск путем вызова метода Cancel() у объекта CancellationTokenSource
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, "Бот успешно остановлен",replyMarkup:MainKeyBoard.BotsPanelNotcoin());
                                }
                                break;
                            // Tehlical support Notcoin bot Button
                            case "mbtn_bots_notcoin_to":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId) 
                                {
                                    if (AdminPanel.adminFlagTO == false)
                                    {
                                        await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                        AdminPanel.adminFlagTO = true;
                                        await bot.SendTextMessageAsync(LocalConfig.adminMainId, "✅ Режим тех обслуживания активирован!", replyMarkup: MainKeyBoard.BotsPanelNotcoin(), parseMode: ParseMode.Html);
                                    }
                                    if(AdminPanel.adminFlagTO == true) 
                                    {
                                        await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                        AdminPanel.adminFlagTO = false;
                                        await bot.SendTextMessageAsync(LocalConfig.adminMainId, "✅ Режим тех обслуживания отключен!", replyMarkup: MainKeyBoard.BotsPanelNotcoin(), parseMode: ParseMode.Html);
                                    }
                                }
                                break;
                            // Exit Button from Notcoin bot Panel
                            case "mbtn_bots_notcoin_back":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = "⚙️ Вы вошли в панель управления ботами всей экосистемы 🌳\n";
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, text, replyMarkup: MainKeyBoard.BotsPanel(), parseMode: ParseMode.Html);
                                }
                                break;
                            // BOTS PANEL ACCEPTER
                            case "mbtn_accepter":
                                await bot.DeleteMessageAsync(LocalConfig.adminMainId, callbackQuery.Message.MessageId);
                                string statusBot = "Остановлен";
                                if (LocalConfig.isAccepterEnable)
                                    statusBot = "Запущен";
                                text = $"🔐 Статус бота: {statusBot}";
                                await bot.SendTextMessageAsync(LocalConfig.adminMainId, text, replyMarkup: MainKeyBoard.BotsPanelAccepter(),parseMode:ParseMode.Html);
                                break;
                            // In Accepter pane; buttons
                            case "mbtn_bots_accepter_status":
                                if (LocalConfig.isAccepterEnable == false)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isAccepterEnable = true;
                                    Task.Run(() => accepter.run.RunAccepter(cancellationToken));
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, "Бот успешно запущен", replyMarkup: MainKeyBoard.BotsPanelAccepter());
                                }
                                if (LocalConfig.isAccepterEnable == true)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isAccepterEnable = false;
                                    cancellationTokenSource.Cancel(); // Остановить таск путем вызова метода Cancel() у объекта CancellationTokenSource
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, "Бот успешно остановлен", replyMarkup: MainKeyBoard.BotsPanelAccepter());
                                }
                                break;
                            case "mbtn_bots_accepter_back":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                text = "⚙️ Вы вошли в панель управления ботами всей экосистемы 🌳\n";
                                await bot.SendTextMessageAsync(LocalConfig.adminMainId, text, replyMarkup: MainKeyBoard.BotsPanel(), parseMode: ParseMode.Html);

                                break;
                                // The Last button on the main menu
                            case "mbtb_status_botnet":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);

                                string notbot = "🌐 Не в сети";
                                string btcbot = "🌐 Не в сети";
                                string accepterBot = "🌐 Не в сети";

                                if (LocalConfig.isBTCEnable)
                                    btcbot = "📶 В сети";
                                if(LocalConfig.isNotcoinRun)
                                    notbot = "📶 В сети";
                                if(LocalConfig.isAccepterEnable)
                                    accepterBot = "📶 В сети";

                                text = $"🖥 Актуальная информация о ботах:\n" +
                                    $"🤖 Notcoin Bot: <b>{notbot}</b>\n" +
                                    $"🤖 Бот заявочник: <b>{accepterBot}</b>\n" +
                                    $"🤖 Btc Bot: <b>{btcbot}</b>";

                                await bot.SendTextMessageAsync(LocalConfig.adminMainId,text,replyMarkup:MainKeyBoard.ExitButton(), parseMode: ParseMode.Html);
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                break;

                            // exit button for ALL PANELS
                            case "mbtn_main_menu_back":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId)
                                { 
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = $"❤️‍🔥 {callbackQuery.Message.Chat.FirstName}, с возвращением!\n" +
                                        $"🤖 Я - главный бот управления всей экосистемой 🌳\n" +
                                        $"⚙️ Разработчик: @WH3BABY\n" +
                                        $"👇 Клавиатура ниже в твоем распоряжении";
                                    await bot.SendTextMessageAsync(LocalConfig.adminMainId, text, replyMarkup: MainKeyBoard.MainMenu(), parseMode: ParseMode.Html);
                                    
                                }
                                break;
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

