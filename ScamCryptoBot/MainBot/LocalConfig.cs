namespace ScamCryptoBot
{
    internal class LocalConfig
    {
        public static int adminMainId = 349421147;
        // FLAGS
        public static bool isNotcoinRun = false;
        public static bool isBTCEnable = false;
        public static bool isAccepterEnable = false;
        public static bool isWorkerEnabled = false;
        public static bool setNewPercent = false;
        public static bool setNewAdmin = false;
        public static bool setNewTP = false;
        // DATA
        public static int PercentOfWorkers = 45;
        public static string btcWalletAddress = "";
        public static string ethWalletAddress = "";
        public static string notcoinWalletAddress = "";

        //

        public static async Task LoadFromConfigFile()
            // TODO 
        {
            string dataFile = "configuration.txt";

            if (File.Exists(dataFile))
            {
                using (StreamReader reader = new StreamReader(dataFile))
                {
                    string line;
                    bool isEmpty = true;  // Flag to check if the configuration file is empty

                    while ((line = reader.ReadLine()) != null)
                    {
                        isEmpty = false;


                    }
                    if (isEmpty)
                    {
                        MainClass.PrintColoredText("[#] The configuration exists, but it is empty.", ConsoleColor.Yellow);
                    }
                }
            }
            else
            {
                MainClass.PrintColoredText("[-] The configuration file does not exist.", ConsoleColor.Red);
            }
        }


    }
}
