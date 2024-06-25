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
                                if (msg.Text == "/start" && chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(chat.Id)))
                                {
                                    await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                    string text = $"❤️‍🔥 {chat.FirstName}, с возвращением!\n" +
                                        $"🤖 Я - главный бот управления всей экосистемой 🌳\n" +
                                        $"⚙️ Разработчик: @WH3BABY\n" +
                                        $"👇 Клавиатура ниже в твоем распоряжении";
                                    await bot.SendTextMessageAsync(chat.Id, text,replyMarkup:MainKeyBoard.MainMenu(),parseMode:ParseMode.Html);
                                    break;
                                }
                                // Procent setter
                                if (LocalConfig.setNewPercent) 
                                {
                                    string newPercent = msg.Text;
                                    int percent = Convert.ToInt32(newPercent);
                                    if (percent > 0) 
                                    {
                                        await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                        LocalConfig.PercentOfWorkers = percent;
                                        await bot.SendTextMessageAsync(chat.Id, $"Готово!\nУстановлен новый процент выплаты: {LocalConfig.PercentOfWorkers}%",replyMarkup:MainKeyBoard.ConfigPanel());
                                    }
                                    LocalConfig.setNewPercent = false;
                                }
                                // New Admin Add
                                if (LocalConfig.setNewAdmin) 
                                {
                                    string id = msg.Text;
                                    int idd = Convert.ToInt32(id);
                                    if (idd > 0) 
                                    {
                                        await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                        DataBase.AddAdminToList(idd);
                                        await bot.SendTextMessageAsync(chat.Id, "Администратор успешно добавлен в базу.", replyMarkup: MainKeyBoard.ConfigPanel());
                                    
                                        LocalConfig.setNewAdmin = false;
                                    }
                                }
                                // New Tp ADD
                                if (LocalConfig.setNewTP)
                                {
                                    string id = msg.Text;
                                    int idd = Convert.ToInt32(id);
                                    if (idd > 0)
                                    {
                                        await bot.DeleteMessageAsync(chat.Id, msg.MessageId);
                                        DataBase.AddTehnicalSupports(idd);
                                        await bot.SendTextMessageAsync(chat.Id, "Пользователь технической поддержки успешно добавлен в базу.", replyMarkup: MainKeyBoard.ConfigPanel());
                                   
                                        LocalConfig.setNewTP = false;
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
                            // Main Menu after command /start
                            case "mbtn_configuration":
                                // Config Panel
                                string text = "⚙️ Вы вошли в меню конфигурации всей экосистемы 🌳\n" +
                                    "Будьте осторожнее с настройками.";
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id))) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id,text,replyMarkup:MainKeyBoard.ConfigPanel(),parseMode:ParseMode.Html);
                                }
                                break;
                            // CONFIGURATOR PANEL BUTTONS
                            case "mbtn_main_percent":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Текущий процент воркера: {LocalConfig.PercentOfWorkers}\nПришлите мне новое значение",parseMode:ParseMode.Html);
                                LocalConfig.setNewPercent = true;
                                break;
                            case "mbtn_main_add_admin":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Пришли ID нового админа");
                                LocalConfig.setNewAdmin = true;
                                break;
                            case "mbtn_main_add_tp":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Пришли ID нового тп");
                                LocalConfig.setNewTP = true;
                                break;
                            case "mbtn_main_wallets":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                // wallets state 
                                string btc = "Не установлен", eth = "Не установлен", not = "Не установлен";
                                if (!string.IsNullOrEmpty(LocalConfig.btcWalletAddress)) btc = "Подключен";
                                if (!string.IsNullOrEmpty(LocalConfig.ethWalletAddress)) eth = "Подключен";
                                if (!string.IsNullOrEmpty(LocalConfig.notcoinWalletAddress)) not = "Подключен";
                                text = $"Текущее состояние кошельков\n" +
                                    $"BTC: {btc}\n" +
                                    $"ETH: {eth}\n" +
                                    $"Notcoin: {not}";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id , text, replyMarkup:MainKeyBoard.ConfiguratorWalletsPanel(),parseMode:ParseMode.Html );
                                break;
                            case "mbtn_config_back":
                                 text = "⚙️ Вы вошли в меню конфигурации всей экосистемы 🌳\n" +
                                    "Будьте осторожнее с настройками.";
                                if (callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id)))
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.ConfigPanel(), parseMode: ParseMode.Html);
                                }
                                break;
                                // Admin panel
                            case "mbtn_admin_panel":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id))) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = "⚙️ Вы вошли в админ панель\nВерсия: 0.1";
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.AdminPanel(), parseMode: ParseMode.Html);
                                }
                                break;
                            // Buttons in APanel
                            case "mbtn_main_backup_database":
                                await DataBase.DataBaseRecover();
                                break;
                                // BotPanel
                            case "mbtn_bots_panel":
                                if(LocalConfig.adminMainId == callbackQuery.Message.Chat.Id || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id))) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = "⚙️ Вы вошли в панель управления ботами всей экосистемы 🌳\n";
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id,text,replyMarkup: MainKeyBoard.BotsPanel(),parseMode: ParseMode.Html);
                                }
                                break;
                                
                            // BOTS PANEL_NOTCOIN
                            case "mbtn_notcoin":
                                
                                // double check ID 
                                if (callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id))) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                     string sstatusBot = "Остановлен";
                                    if (LocalConfig.isNotcoinRun)
                                        sstatusBot = "Запущен";
                                    text = $"🔐 Статус бота: {sstatusBot}";
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.BotsPanelNotcoin(),parseMode:ParseMode.Html);
                                }
                                break;
                            // start stop Notcoin bot
                            case "mbtn_bots_notcoin_status":
                                if (LocalConfig.isNotcoinRun == false)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isNotcoinRun = true;
                                    Task.Run(() => notcoin.Run.RunNotcoinScamBot(cancellationToken));
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Бот успешно запущен", replyMarkup: MainKeyBoard.BotsPanelNotcoin());
                                }
                                if(LocalConfig.isNotcoinRun == true)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isNotcoinRun = false;
                                    cancellationTokenSource.Cancel(); // Остановить таск путем вызова метода Cancel() у объекта CancellationTokenSource
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Бот успешно остановлен",replyMarkup:MainKeyBoard.BotsPanelNotcoin());
                                }
                                break;
                            // Tehlical support Notcoin bot Button
                            case "mbtn_bots_notcoin_to":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id))) 
                                {
                                    if (AdminPanel.adminFlagTO == false)
                                    {
                                        await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                        AdminPanel.adminFlagTO = true;
                                        await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "✅ Режим тех обслуживания активирован!", replyMarkup: MainKeyBoard.BotsPanelNotcoin(), parseMode: ParseMode.Html);
                                    }
                                    if(AdminPanel.adminFlagTO == true) 
                                    {
                                        await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                        AdminPanel.adminFlagTO = false;
                                        await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "✅ Режим тех обслуживания отключен!", replyMarkup: MainKeyBoard.BotsPanelNotcoin(), parseMode: ParseMode.Html);
                                    }
                                }
                                break;
                            // Exit Button from Notcoin bot Panel
                            case "mbtn_bots_notcoin_back":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id))) 
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = "⚙️ Вы вошли в панель управления ботами всей экосистемы 🌳\n";
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.BotsPanel(), parseMode: ParseMode.Html);
                                }
                                break;
                            // BOTS PANEL ACCEPTER
                            case "mbtn_accepter":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                string statusBot = "Остановлен";
                                if (LocalConfig.isAccepterEnable)
                                    statusBot = "Запущен";
                                text = $"🔐 Статус бота: {statusBot}";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.BotsPanelAccepter(),parseMode:ParseMode.Html);
                                break;
                            // In Accepter pane; buttons
                            case "mbtn_bots_accepter_status":
                                if (LocalConfig.isAccepterEnable == false)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isAccepterEnable = true;
                                    Task.Run(() => accepter.run.RunAccepter(cancellationToken));
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Бот успешно запущен", replyMarkup: MainKeyBoard.BotsPanelAccepter());
                                }
                                if (LocalConfig.isAccepterEnable == true)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isAccepterEnable = false;
                                    cancellationTokenSource.Cancel(); // Остановить таск путем вызова метода Cancel() у объекта CancellationTokenSource
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Бот успешно остановлен", replyMarkup: MainKeyBoard.BotsPanelAccepter());
                                }
                                break;
                            case "mbtn_bots_accepter_back":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                text = "⚙️ Вы вошли в панель управления ботами всей экосистемы 🌳\n";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.BotsPanel(), parseMode: ParseMode.Html);
                                break;
                            // BOT PANEL WORKER BOT
                            case "mbtn_worker_bot":
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                statusBot = "Остановлен";
                                if (LocalConfig.isWorkerEnabled)
                                    statusBot = "Запущен";
                                text = $"🔐 Статус бота: {statusBot}";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.BotsPanelWorker(), parseMode: ParseMode.Html);
                                break;
                                // Worker bot in bots panel
                            case "mbtn_bots_worker_status":
                                if (LocalConfig.isWorkerEnabled == false)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isWorkerEnabled = true;
                                    Task.Run(() => worker.Run.RunWorkerBot(cancellationToken));
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Бот успешно запущен", replyMarkup: MainKeyBoard.BotsPanelWorker());
                                }
                                if (LocalConfig.isWorkerEnabled == true)
                                {
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    LocalConfig.isWorkerEnabled = false;
                                    cancellationTokenSource.Cancel(); // Остановить таск путем вызова метода Cancel() у объекта CancellationTokenSource
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Бот успешно остановлен", replyMarkup: MainKeyBoard.BotsPanelWorker());
                                }
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

                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id,text,replyMarkup:MainKeyBoard.ExitButton(), parseMode: ParseMode.Html);
                                await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                break;

                            // exit button for ALL PANELS
                            case "mbtn_main_menu_back":
                                if(callbackQuery.Message.Chat.Id == LocalConfig.adminMainId || DataBase.IsAdmin(Convert.ToInt32(callbackQuery.Message.Chat.Id)))
                                { 
                                    await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                                    text = $"❤️‍🔥 {callbackQuery.Message.Chat.FirstName}, с возвращением!\n" +
                                        $"🤖 Я - главный бот управления всей экосистемой 🌳\n" +
                                        $"⚙️ Разработчик: @WH3BABY\n" +
                                        $"👇 Клавиатура ниже в твоем распоряжении";
                                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: MainKeyBoard.MainMenu(), parseMode: ParseMode.Html);
                                    
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

