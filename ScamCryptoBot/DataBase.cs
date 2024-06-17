using Telegram.Bot;

namespace ScamCryptoBot
{
    internal class DataBase
    {
        
        public static List<int> UserDataBase = new List<int>();
        public static List<int> BanIDs = new List<int>();
        public static List<string> wallets = new List<string>();
        public static List<(int, string)> WaitList = new List<(int, string)>();
        public static async void AddToWaitList(int userToReg,string userName) 
        {
            if (WaitList.Contains((userToReg, userName))) 
            {
                await accepter.run.telegramClient.SendTextMessageAsync(userToReg, "⚠ Вы уже отправили заявку!\nОжидайте решения.");
                return;
            }
                
            WaitList.Add((userToReg, userName));

        }
        public static void RemoveFromWaitList(int id,string userName) 
        {
            WaitList.Remove((id,userName));
        }
        public static void AddWalletToConfig(string wallet)
        {
            wallets.Add(wallet); // 0 - NOT // 1 - BTC // 2 - ETH
        }
        // Primitive register
        public static void addToDataBase(int userId)
        {
            if (UserDataBase.Contains(userId))
            {
                return;
            }
            else
                UserDataBase.Add(userId);
        }
        public static void addToBanIDs(int userId)
        {
            if (BanIDs.Contains(userId))
            { return; }
            BanIDs.Add(userId);
            UserDataBase.Remove(userId);
        }

        public static int GetAllUsers()
        {
            return UserDataBase.Count;
        }
        public static int GetAllBanesCount()
        {
            return BanIDs.Count;
        }
        // Backup
        public static async Task DataBaseRecover()
        {
            string dataFile = "database.txt";
            using (StreamWriter writer = new StreamWriter(dataFile))
            {
                foreach (int userId in UserDataBase)
                {
                    await writer.WriteLineAsync($"UserID: {userId}");
                }
                foreach (int banId in BanIDs)
                {
                    await writer.WriteLineAsync($"BannedID: {banId}");
                }
            }

        }
        // Load
        public static async Task LoadFromDataBase()
        {
            string dataFile = "database.txt";

            if (File.Exists(dataFile))
            {
                using (StreamReader reader = new StreamReader(dataFile))
                {
                    string line;
                    bool isEmpty = true;  // Flag to check if the database file is empty

                    while ((line = reader.ReadLine()) != null)
                    {
                        isEmpty = false;

                        if (line.StartsWith("UserID: "))
                        {
                            string userIdString = line.Substring(8);
                            if (int.TryParse(userIdString, out int userId))
                            {
                                addToDataBase(userId);
                            }
                        }
                        else if (line.StartsWith("BannedID: "))
                        {
                            string banIdString = line.Substring(10);
                            if (int.TryParse(banIdString, out int banId))
                            {
                                addToBanIDs(banId);
                            }
                        }
                    }

                    if (isEmpty)
                    {
                        MainClass.PrintColoredText("[#] The database exists, but it is empty.",ConsoleColor.Yellow);
                    }
                }
            }
            else
            {
                MainClass.PrintColoredText("[-] The database file does not exist.",ConsoleColor.Red);
            }
        }

    }
}
