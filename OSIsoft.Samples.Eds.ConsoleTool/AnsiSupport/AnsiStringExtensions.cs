// // // <copyright file="AnsiStringExtensions.cs" company="OSIsoft, LLC">
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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport
{
    /// <summary>
    ///     Allows any string to be converted to an ANSI string, using BB style codes. See AnsiConverter for options and color
    ///     definitions.
    /// </summary>
    public static class AnsiStringExtensions
    {
        public static string ToAnsi(this string str)
        {
            return AnsiConverter.Convert(str);
        }


        public static string ToAnsiBold(this string str)
        {
            return $"[b]{str}[/b]".ToAnsi();
        }

        public static string ToAnsiDebug(this string str)
        {
            return $"[d]{str}[/d]".ToAnsi();
        }

        public static string ToAnsiInfo(this string str)
        {
            return $"[i]{str}[/i]".ToAnsi();
        }

        public static string ToAnsiSuccess(this string str)
        {
            return $"[s]{str}[/s]".ToAnsi();
        }

        public static string ToAnsiWarning(this string str)
        {
            return $"[w]{str}[/w]".ToAnsi();
        }

        public static string ToAnsiError(this string str)
        {
            return $"[e]{str}[/e]".ToAnsi();
        }

        public static string ToAnsiReversed(this string str)
        {
            return $"[r]{str}[/r]".ToAnsi();
        }

        public static string ToAnsiUnderline(this string str)
        {
            return $"[u]{str}[/u]".ToAnsi();
        }

        public static string SyntaxHighlightJson(this string original)
        {
            //Adapted from: http://joelabrahamsson.com/syntax-highlighting-json-with-c/
            //Credit: Joel Abrahamsson (7-APR-2012)
            return Regex.Replace(
                original,
                @"(¤(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\¤])*¤(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)"
                    .Replace('¤', '"'),
                match =>
                {
                    if (Regex.IsMatch(match.Value, @"^¤".Replace('¤', '"')))
                    {
                        if (Regex.IsMatch(match.Value, ":$"))
                            //cls = "key";
                            return $"[b]{match}[/b]";
                        return $"[i]{match}[/i]";
                    }

                    if (Regex.IsMatch(match.Value, "true"))
                        //cls = "boolean";
                        return $"[s]{match}[/s]";
                    if (Regex.IsMatch(match.Value, "false"))
                        //cls = "boolean";
                        return $"[e]{match}[/e]";
                    if (Regex.IsMatch(match.Value, "null"))
                        //cls = "null";
                        return $"[d]{match}[/d]";
                    return $"[w]{match}[/w]";
                }).ToAnsi();
        }

        /// <summary>
        ///     Finds all the indexes of an occurence in a string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                return new List<int>();
            var indexes = new List<int>();
            for (var index = 0;; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}