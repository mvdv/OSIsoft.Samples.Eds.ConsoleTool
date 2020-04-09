// // // <copyright file="StreamCreatorWizard.cs" company="OSIsoft, LLC">
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
using System.Linq;
using System.Threading.Tasks;
using OSIsoft.Data;
using OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport;

namespace OSIsoft.Samples.Eds.ConsoleTool.Wizards
{
    /// <summary>
    /// Wizard to allow a user to create an SDS Stream
    /// </summary>
    public class StreamCreatorWizard
    {
        public async Task<SdsStream> Create(ISdsMetadataService metadataService)
        {
            var rnd = new Random();
            var stream = new SdsStream();

            Console.WriteLine("Create new SDS stream".ToAnsiBold());
            Console.WriteLine(
                "This wizard will take you through the steps to create a new SDS stream. The default answers are [i]highlighted[/i]");
            Console.WriteLine();
            stream.Id = ConsoleHelpers.AskQuestion("ID", "NewStream" + rnd.Next(0, 1000));
            stream.Name = ConsoleHelpers.AskQuestion("Name", stream.Id);
            stream.Description = ConsoleHelpers.AskQuestion("Description", $"{stream.Name} description");


            var types = await metadataService.GetTypesAsync();

            var typeIds = types.Select(t => t.Id).ToList();

            var selection = ConsoleHelpers.AskOptions("Type", typeIds);

            stream.TypeId = selection.text;

            return stream;
        }
    }
}