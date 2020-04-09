// // // <copyright file="WindowsAnsiSupport.cs" company="OSIsoft, LLC">
// // //   Copyright (c) 2020 OSIsoft, LLC.  All rights reserved.
// // //
// // //   THIS SOFTWARE CONTAINS CONFIDENTIAL INFORMATION AND TRADE SECRETS OF
// // //   OSIsoft, LLC.  USE, DISCLOSURE, OR REPRODUCTION IS PROHIBITED WITHOUT
// // //   THE PRIOR EXPRESS WRITTEN PERMISSION OF OSIsoft, LLC.
// // //
// // //   RESTRICTED RIGHTS LEGEND
// // //   Use, duplication, or disclosure by the Government is subject to restrictions
// // //   as set forth in subparagraph (c)(1)(ii) of the Rights in Technical Data and
// // //   Computer Software clause at DFARS 252.227.7013
// // //
// // //   OSIsoft, LLC
// // //   1600 Alvarado Street. San Leandro, CA  94577 USA
// // // </copyright>

using System;
using System.Runtime.InteropServices;

namespace OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport
{
    /// <summary>
    ///     Enables support for ANSI in the Windows 10 Console. This is not enabled by default. We use ANSI codes to decorate
    ///     some of our output.
    ///     This code was produced by Jerrie Pelser
    ///     (https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/)
    /// </summary>
    internal class WindowsAnsiSupport
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static void Enable()
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out var outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
            }
        }
    }
}