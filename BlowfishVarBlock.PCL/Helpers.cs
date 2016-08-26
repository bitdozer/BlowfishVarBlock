using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlowfishVarBlock.PCL
{
    public static class Helpers
    {
        public static void DumpBytes(this byte[] vector, string message = null)
        {
            string sBytes = "";
            string aBytes = "";
            string hBytes = "";
            foreach (var element in vector)
            {
                sBytes += String.Format("0x{0:x2}, ", element);
                hBytes += String.Format("{0:x2}", element);
                aBytes += String.Format("{0}, ", element);
            }
            if (!string.IsNullOrWhiteSpace(message))
                Debug.WriteLine(message);
            Debug.WriteLine(sBytes.Trim().Trim(','));
            Debug.WriteLine(aBytes.Trim().Trim(','));
            Debug.WriteLine(hBytes.Trim().Trim(','));
        }
        public static void DumpBinary(this ulong u, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                Debug.WriteLine(message);
            Debug.WriteLine(GetBinary((uint)(u >> 32)) + " " + GetBinary((uint)u));
        }
        public static void DumpBinary(uint lr, uint rr, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                Debug.WriteLine(message);
            Debug.WriteLine(GetBinary(lr) + " " + GetBinary(rr)); ;
        }
        public static void DumpBinary(this uint u, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                Debug.WriteLine(message);
            Debug.WriteLine(GetBinary(u));
        }
        public static string GetBinary(uint u)
        {
            //return Convert.ToString((uint)u, 2).PadLeft(32, '0');
            return GetBinary((byte)(u >> 24)) + " " + GetBinary((byte)(u >> 16)) + " " + GetBinary((byte)(u >> 8)) + " " + GetBinary((byte)(u));
        }
        public static string GetBinary(byte u)
        {
            return Convert.ToString(u, 2).PadLeft(8, '0');
        }
    }
}
