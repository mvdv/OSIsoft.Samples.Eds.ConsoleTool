// // // <copyright file="ConsoleSpinner.cs" company="OSIsoft, LLC">
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
using System.Timers;

namespace OSIsoft.Samples.Eds.ConsoleTool
{
    /// <inheritdoc />
    /// <summary>
    ///     Displays a continues spinner on the console to indicate a busy marker.
    ///     Adapted from example on StackOverflow (https://stackoverflow.com/questions/1923323/console-animations)
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public class ConsoleSpinner : IDisposable
    {
        private const string Sequence = @"/-\|";
        private readonly int left;

        private readonly Timer timer;

        private readonly int top;
        private int counter;

        public ConsoleSpinner(int left, int top, int delay = 100)
        {
            this.left = left;
            this.top = top;
            this.timer = new Timer(delay);
            this.timer.Elapsed += (o, e) => this.Turn();
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }


        public void Start()
        {
            if (!this.timer.Enabled)
                this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
            this.Draw(' ');
        }


        private void Draw(char c)
        {
            Console.SetCursorPosition(this.left, this.top);

            Console.Write(c);
        }

        private void Turn()
        {
            this.Draw(Sequence[++this.counter % Sequence.Length]);
        }
    }
}