// // // <copyright file="AnsiConverter.cs" company="OSIsoft, LLC">
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
using System.Collections.Generic;
using System.Text;

namespace OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport
{
    /// <summary>
    ///     Converts BB-style markup language to ANSI. Used by the AnsiStringExtensions class. The markup loosely follows
    ///     Bootstraps style markup.
    ///     Markup follows the [b]text[/b] structure (example bold)
    /// </summary>
    public class AnsiConverter
    {
        /// <summary>
        ///     ANSI sequence to end all previous markup
        /// </summary>
        private const string EndSequence = "\u001b[0m";

        /// <summary>
        ///     Lookup for all the style codes. If you want to change on how markup is represented, you can do it here.
        /// </summary>
        private static readonly Dictionary<string, string> Codes =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"b", "\u001b[1m"}, //bold
                {"i", "\u001b[36;1m"}, //info (Cyan)
                {"d", "\u001b[30;1m"}, //debug (Bright black)
                {"s", "\u001b[32m"}, //success (Green)
                {"w", "\u001b[33m"}, //warning (Yellow)
                {"e", "\u001b[31;1m"}, //error (Bright red),
                {"u", "\u001b[4m"}, //underline,
                {"r", "\u001b[7m"} //reversed
                //  { "i", "\u001b[38;5;206m" }
            };

        /// <summary>
        ///     Converts the specified input from BB-style markup to ANSI
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Convert(string input)
        {
            var sb = new StringBuilder(input);
            foreach (var code in Codes)
            {
                sb.Replace($"[{code.Key}]", code.Value);
                sb.Replace($"[/{code.Key}]", EndSequence);
            }


            return sb.ToString();
        }
    }
}