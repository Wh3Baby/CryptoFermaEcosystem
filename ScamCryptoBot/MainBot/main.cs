using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;


namespace ScamCryptoBot
{
    internal class MainClass
    {
        private static ITelegramBotClient telegramClient;
        private static ReceiverOptions receiverOptions;
        private static string tokenBot;
        private static string CryptoFermaVersion = " 1.1.3";

        static async Task Main(string[] args)
        {
            
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 12 * 60 * 60 * 1000; // 12 часов в миллисекундах
            timer.Elapsed += TimerElapsed;
            timer.Start();

            tokenBot = "7037433641:AAG_Cpv9icgQMbm0AUD5E2zVtCUOvJfQfkM"; // Создать ручной ввод токена с клавы, либо с файла конфигурации

            telegramClient = new TelegramBotClient(tokenBot);
            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery,
                },
                ThrowPendingUpdates = true,
            };
            // Обновляем значения статусов ботов
            string notcoinBotStatus = LocalConfig.isNotcoinRun ? "Enabled" : "Disabled";
            string btcBotStatus = LocalConfig.isBTCEnable ? "Enabled" : "Disabled";
            string accepterBotStatus = LocalConfig.isAccepterEnable ? "Enabled" : "Disabled";
            using var cts = new CancellationTokenSource();
            telegramClient.StartReceiving(MainHandlers.UpdateHandler,MainHandlers.ErrorHandler,receiverOptions,cts.Token);
            Console.Title = $"CryptoFerma: {CryptoFermaVersion}";
            PrintColoredText($"[#] Check database.....", ConsoleColor.Cyan);
            await DataBase.LoadFromDataBase();
            int workersCount = DataBase.UserDataBase.Count;
            int bansCount = DataBase.BanIDs.Count;
            PrintColoredText($"[+] Bot sucsessuly startet at: {tokenBot}\n" +
                $"[+] Version:{CryptoFermaVersion}\n" +
                $"[#] Admin ID: {LocalConfig.adminMainId}\n" +
                $"[#] Notcoin Bot status: {notcoinBotStatus}\n" +
                $"[#] BTC Bot status: {btcBotStatus}\n" +
                $"[#] Accepter bot status: {accepterBotStatus}\n" +
                $"[#] Workers in DataBase: {workersCount}\n" +
                $"[#] Ban users in DataBase: {bansCount}" +
                $"[#] Workers percent: {LocalConfig.PercentOfWorkers}%\n" +
                $"[#] Develop by AM0R3M10: https://t.me/WH3BABY",ConsoleColor.DarkMagenta);
            await Task.Delay(-1); // В таком состоянии он будет вечно запущен , можно установить на автоматике кол-во дней 
                                  // к примеру аренда всего проекта на 30 дней == await Task.Delay(TimeSpan.FromDays(30));



        }
        public static void PrintColoredText(string messge, ConsoleColor consoleColor) 
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(messge);
            Console.ResetColor();
        }
        // DataBase BackUp after 12h works..
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
           DataBase.DataBaseRecover().GetAwaiter().GetResult();
        }
    }
}
