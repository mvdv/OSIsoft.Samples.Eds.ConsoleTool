// // // <copyright file="Program.cs" company="OSIsoft, LLC">
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
using System.Threading.Tasks;
using OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport;

namespace OSIsoft.Samples.Eds.ConsoleTool
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //Check if we are running on Windows. If so, enable ANSI support for Windows. For Linux, this is not needed.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WindowsAnsiSupport.Enable();

            //Start the console tools
            var edsConsoleTools = new EdsConsoleTools();
            await edsConsoleTools.Run();

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}