// // // <copyright file="TypeListWizard.cs" company="OSIsoft, LLC">
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

using ConsoleTables;
using OSIsoft.Data;

namespace OSIsoft.Samples.Eds.ConsoleTool.Wizards
{
    /// <summary>
    /// Small wizard to allow a user to search for and list SDS Types
    /// </summary>
    public class TypeListWizard
    {
        public void Run(ISdsMetadataService service)
        {
            var query = ConsoleHelpers.AskQuestion("Query?", "*");
            var count = ConsoleHelpers.AskQuestion("Count?", "10");
            var skip = ConsoleHelpers.AskQuestion("Skip?", "0");

            var t = service.GetTypesAsync(query, int.Parse(skip), int.Parse(count));
            ConsoleHelpers.ExecuteWhileSpinning(t);
            var types = t.Result;


            ConsoleTable.From(types).Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Minimal);
        }
    }
}