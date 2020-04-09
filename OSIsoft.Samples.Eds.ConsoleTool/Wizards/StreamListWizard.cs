// // // <copyright file="StreamListWizard.cs" company="OSIsoft, LLC">
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
using System.Threading.Tasks;
using ConsoleTables;
using OSIsoft.Data;

namespace OSIsoft.Samples.Eds.ConsoleTool.Wizards
{
    public class StreamListWizard
    {
        public void Run(ISdsMetadataService service)
        {
            var query = ConsoleHelpers.AskQuestion("Query?", "*");
            var count = ConsoleHelpers.AskQuestion("Count?", "10");
            var skip = ConsoleHelpers.AskQuestion("Skip?", "0");

            var t = service.GetStreamsAsync(query, int.Parse(skip), int.Parse(count));
            ConsoleHelpers.ExecuteWhileSpinning(t);
            var streams = t.Result;


            ConsoleTable.From(streams).Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Minimal);
        }
    }
}