using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace ScamCryptoBot.accepter
{
    internal class keyboard
    {
        public static InlineKeyboardMarkup RegisterKeys()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("💼Оставить заявку🚀","button_reg")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("🚫Отмена🚫","button_stop")
                    },
                    
                }
                );
            return inlineKeyboard;
        }
        // Show this keyboard admins or tehlical supports
        public static InlineKeyboardMarkup AcceptKeys() 
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("✅ Принять ✅","button_accpet")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("🚫 Отказать 🚫","button_denid")
                    },

                }
                );
            return inlineKeyboard;
        }
    }
}
