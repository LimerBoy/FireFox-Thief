/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/FireFox-Thief
*/

using System;
using System.Text.RegularExpressions;

namespace Stealer.Helpers
{
    internal sealed class Json
    {
        public string Data;
        public Json(string data)
        {
            this.Data = data;
        }
        // Get string value from json dictonary
        public string GetValue(string value)
        {
            string result = String.Empty;
            Regex valueRegex = new Regex($"\"{value}\":\"([^\"]+)\"");
            Match valueMatch = valueRegex.Match(this.Data);
            if (!valueMatch.Success)
                return result;

            result = Regex.Split(valueMatch.Value, "\"")[3];
            return result;
        }
        // Remove string
        public void Remove(string[] values)
        {
            foreach (string value in values)
                this.Data = this.Data.Replace(value, "");
        }
        // Get array from json data
        public string[] SplitData(string delimiter = "},")
        {
            return Regex.Split(this.Data, delimiter);
        }
    }
}
