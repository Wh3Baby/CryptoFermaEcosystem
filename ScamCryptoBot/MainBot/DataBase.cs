using Telegram.Bot;

namespace ScamCryptoBot
{
    internal class DataBase
    {
        
        public static List<int> UserDataBase = new List<int>();
        public static List<int> BanIDs = new List<int>();
        public static List<(int, string)> WaitList = new List<(int, string)>();
        public static List<int> AdminList = new List<int>();
        public static List<int> TehnicalSupports = new List<int>();

        public static void AddTehnicalSupports(int id) 
        {
            if (TehnicalSupports.Contains(id)) { return; }
            TehnicalSupports.Add(id);
        }
        public static bool IsAdmin(int id) 
        {
            foreach (int i in AdminList)
                {
                if(i == id) { return true; }
            }
            return false;
        }
        public static void AddAdminToList (int id) 
        {
            AdminList.Add (id);
        }
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
                foreach (int adminID in AdminList) 
                {
                    await writer.WriteLineAsync($"AdminID: {adminID}");
                }
                foreach(int thID in TehnicalSupports) 
                {
                    await writer.WriteAsync($"TpID: {thID}");
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
                        else if(line.StartsWith("AdminID: ")) 
                        {
                            string adminID = line.Substring(9);
                            if(int.TryParse(adminID, out int adminId)) 
                            {
                                AddAdminToList(adminId);    
                            }
                        }
                        else if (line.StartsWith("TpID: ")) 
                        {
                            string TpID = line.Substring(6);
                            if(int.TryParse(TpID, out int TpiD)) 
                            {
                                AddTehnicalSupports(TpiD);
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
        // TelegramID in chat == worker id? in database
        public static bool IsWorker(int workerId) 
        {
            foreach(int idS in UserDataBase) 
            {
                if (idS == workerId) 
                {
                    
                    return true;
                }
            }
            return false;
        }
        // thID == ban id in databse BanID?
        public static bool isBanned(int id) 
        {
            foreach (int ids in BanIDs) 
            {
                if(ids == id) 
                {
                    return true;
                }

            }
            return false;
        }

    }
}
