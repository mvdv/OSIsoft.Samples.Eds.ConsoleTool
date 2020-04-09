// // // <copyright file="ConsoleHelpers.cs" company="OSIsoft, LLC">
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport;

namespace OSIsoft.Samples.Eds.ConsoleTool
{
    /// <summary>
    ///     Helper class to interact with the user on the console.
    /// </summary>
    public static class ConsoleHelpers
    {
        /// <summary>
        ///     Determines whether a string is affirmative
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <returns>
        ///     <c>true</c> if [is affirmative answer] [the specified answer]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAffirmativeAnswer(string answer)
        {
            if (answer.ToLower() == "no" || answer.ToLower() == "n")
                return false;
            if (string.IsNullOrWhiteSpace(answer) || answer.ToLower() == "y" || answer.ToLower() == "yes")
                return true;
            return false;
        }

        /// <summary>
        ///     Displays a busy marker (spinner) while executing the specified Action
        /// </summary>
        /// <param name="action">The action.</param>
        public static void ExecuteWhileSpinning(Action action)
        {
            var t = Task.Run(action);
            var spinner = new ConsoleSpinner(Console.CursorLeft, Console.CursorTop);
            spinner.Start();
            Task.WaitAll(t);
            spinner.Stop();
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a busy marker (spinner) while executing the specified Task
        /// </summary>
        /// <param name="t"></param>
        public static void ExecuteWhileSpinning(Task t)
        {
            var spinner = new ConsoleSpinner(Console.CursorLeft, Console.CursorTop);
            spinner.Start();
            Task.WaitAll(t);
            spinner.Stop();
            Console.WriteLine();
        }


        /// <summary>
        ///     Asks a basic open ended question with a default answer
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defaultAnswer">The default answer.</param>
        /// <returns></returns>
        public static string AskQuestion(string question, string defaultAnswer)
        {
            Console.Write($"{question} [{defaultAnswer.ToAnsiInfo()}]: ");
            var answer = Console.ReadLine();
            return string.IsNullOrWhiteSpace(answer) ? defaultAnswer : answer;
        }

        /// <summary>
        ///     Asks an open question
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        public static string AskQuestion(string question)
        {
            Console.Write($"{question}: ");
            var answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(question))
                return AskQuestion(question);
            return answer;
        }

        /// <summary>
        ///     Asks a yes or no question with a default answer
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defaultAnswer">if set to <c>true</c> [default answer].</param>
        /// <returns></returns>
        public static bool AskYesNoQuestion(string question, bool defaultAnswer = false)
        {
            Console.Write($"{question} [{(defaultAnswer ? "Y" : "N").ToAnsiInfo()}]: ");
            var answer = Console.ReadLine();
            return IsAffirmativeAnswer(string.IsNullOrEmpty(answer) ? defaultAnswer ? "Y" : "N" : answer);
        }

        /// <summary>
        ///     Asks a multiple choice question from a list of options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="question">The question.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static (int index, T text) AskOptions<T>(string question, IList<T> options)
        {
            Console.WriteLine(question);
            for (var i = 0; i < options.Count; i++)
                Console.WriteLine($"\t{i}. {options[i]}");
            Console.Write("Option: ");
            var choice = Console.ReadLine();

            if (int.TryParse(choice, out var index) && index >= 0 && index < options.Count)
                return (index, options[index]);
            return AskOptions(question, options);
        }

        /// <summary>
        ///     Asks a multiple choice question with options from an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="question">The question.</param>
        /// <param name="defaultOption"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">T must be an enum</exception>
        public static T AskOptions<T>(string question, T defaultOption)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum");

            var options = ((T[]) Enum.GetValues(typeof(T))).ToList();
            var comparer = Comparer<T>.Default;

            var maxWidth = options.Select(o => o.ToString().Length).Max();

            Console.WriteLine(question);
            Console.WriteLine();

            List<string[]> lines = new List<string[]>();

            var numColumns = options.Count / 15 + 1;

            var builder = new StringBuilder();
            for (int i = 0; i < options.Count; i += numColumns)
            {
                var line = "";
                for (int x = 0; x < numColumns; x++)
                    if (i + x < options.Count)
                    {
                        if (comparer.Compare(options[i + x], defaultOption) == 0)
                            line += (i + x + ". " + ("[i]" + options[i + x] + "[/i]").ToAnsi()).PadRight(
                                maxWidth + 4 + 11);
                        else
                            line += (i + x + ". " + options[i + x]).PadRight(maxWidth + 4);
                    }

                builder.AppendLine(line);
            }

            Console.Write(builder.ToString().ToAnsi());

            Console.Write("Option: ");
            var choice = Console.ReadLine();

            if (int.TryParse(choice, out var index) && index >= 0 && index < options.Count)
                return (T) Enum.Parse(typeof(T), options[index].ToString(CultureInfo.CurrentCulture), true);
            return defaultOption;
        }
    }

}