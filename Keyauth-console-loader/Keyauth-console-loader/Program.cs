using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace KeyAuth
{
    class Program
    {

        /*
        * 
        * WATCH THIS VIDEO TO SETUP APPLICATION: https://youtube.com/watch?v=RfDTdiBq4_o
        * 
        * READ HERE TO LEARN ABOUT KEYAUTH FUNCTIONS https://github.com/KeyAuth/KeyAuth-CSHARP-Example#keyauthapp-instance-definition
        *
        */

        public static api KeyAuthApp = new api( 
        name: "",
        ownerid: "",
        secret: "",
        version: "1.0" /*,
        path: @"Your_Path_Here" */ // see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s
        );

        // This will display how long it took to make a request in ms. The param "type" is for "login", "register", "init", etc... but that is optional, as well as this function. Ideally you can just put Console.WriteLine($"Request took {api.responseTime}"), but either works. 
        // if you would like to use this method, simply put it in any function and pass the param ... ShowResponse("TypeHere");
        private void ShowResponse(string type)
        {
            Console.WriteLine($"It took {api.responseTime} ms to {type}");
        }

        static void Main(string[] args)
        {
            Console.Title = "Keyauth Loader";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n Connecting..");
            KeyAuthApp.init();
            Console.Clear();

            //autoUpdate();

            if (!KeyAuthApp.response.success)
            {
                Console.WriteLine("\n Status: " + KeyAuthApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"                       _                    _           ");
            Console.WriteLine(@"                      | |    ___   __ _  __| | ___ _ __ ");
            Console.WriteLine(@"                      | |   / _ \\ / _` |/ _` |/ _ \\ '__|");
            Console.WriteLine(@"                      | |__| (_) | (_| | (_| |  __/ |   ");
            Console.WriteLine(@"                      |_____\\___/ \\__,_|\\__,_|\\___|_|   ");
            Console.WriteLine(@"                                                        ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n                      1)");
            Console.ForegroundColor= ConsoleColor.Blue;
            Console.Write(" Use Key\n\n                      Choose option: ");

            string key;
            //string username, password, key, email; // ONLY USE IF U HAVE LOGIN OPTION

            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.Write("Key: ");
                    key = Console.ReadLine();
                    KeyAuthApp.license(key);
                    break;
                    // invaild option
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n Invalid Selection");
                    Thread.Sleep(2500);
                    Environment.Exit(0);
                    break; // no point in this other than to not get error from IDE
            }

            if (!KeyAuthApp.response.success)
            {
                Console.WriteLine("\n Status: " + KeyAuthApp.response.message);
                Thread.Sleep(2500);
                Environment.Exit(0);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Logged In!"); Console.Clear(); // at this point, the client has been authenticated. Put the code you want to run after here

            // user data
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n User data:");
            Console.WriteLine(" Username: " + KeyAuthApp.user_data.username);
            //Console.WriteLine(" IP address: " + KeyAuthApp.user_data.ip);
            //Console.WriteLine(" Hardware-Id: " + KeyAuthApp.user_data.hwid);
            Console.WriteLine(" Created at: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.createdate)));
            if (!String.IsNullOrEmpty(KeyAuthApp.user_data.lastlogin)) // don't show last login on register since there is no last login at that point
                Console.WriteLine(" Last login at: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.lastlogin)));
            Console.WriteLine(" Your subscription(s):");
            for (var i = 0; i < KeyAuthApp.user_data.subscriptions.Count; i++)
            {
                Console.WriteLine(" Subscription name: " + KeyAuthApp.user_data.subscriptions[i].subscription + " - Expires at: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.subscriptions[i].expiry)) + " - Time left in seconds: " + KeyAuthApp.user_data.subscriptions[i].timeleft);
            }

            Console.WriteLine("\n Closing in five seconds...");
            Thread.Sleep(5000);
            Environment.Exit(0);
        }

        public static bool SubExist(string name)
        {
            if (KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;
            return false;
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            try
            {
                dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            }
            catch
            {
                dtDateTime = DateTime.MaxValue;
            }
            return dtDateTime;
        }

        

        static string random_string()
        {
            string str = null;

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                str += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))).ToString();
            }
            return str;
        }
    }
}
