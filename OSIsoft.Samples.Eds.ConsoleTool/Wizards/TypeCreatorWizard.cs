// // // <copyright file="TypeCreatorWizard.cs" company="OSIsoft, LLC">
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
using OSIsoft.Data;
using OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport;

namespace OSIsoft.Samples.Eds.ConsoleTool.Utilities
{
    /// <summary>
    ///     Wizard to allow a user to create a simple SdsType, including properties.
    /// </summary>
    public class TypeCreatorWizard
    {
        /// <summary>
        ///     Runs the wizard
        /// </summary>
        /// <returns></returns>
        public SdsType Run()
        {
            //TODO: add name checks
            var type = new SdsType();
            var rnd = new Random();

            //Guide the user through the steps required
            Console.WriteLine("Create new SDS Type".ToAnsiBold());
            Console.WriteLine();
            Console.WriteLine(
                "This wizard will take you through the steps to create a new SDS type. The default answers are [i]highlighted[/i]"
                    .ToAnsi());
            Console.WriteLine();

            type.Id = ConsoleHelpers.AskQuestion("ID", "NewType" + rnd.Next(0, 1000));
            type.Name = ConsoleHelpers.AskQuestion("Name", type.Id);
            type.Description = ConsoleHelpers.AskQuestion("Description", $"{type.Name} description");
            type.SdsTypeCode = ConsoleHelpers.AskOptions("Type", SdsTypeCode.Object);
            type.InterpolationMode = ConsoleHelpers.AskOptions("Interpolation mode", SdsInterpolationMode.Default);
            type.ExtrapolationMode = ConsoleHelpers.AskOptions("Extrapolation mode", SdsExtrapolationMode.None);


            //It is now time to start adding properties to our new type. Because we can add many properties, execute this in a loop and ask the user if they want to add another type
            type.Properties = new List<SdsTypeProperty>();
            var addAnother = true;
            Console.WriteLine("You can now add a property".ToAnsiSuccess());
            while (addAnother)
            {
                var property = this.GetTypeProperty();
                if (property != null) type.Properties.Add(property);
                addAnother = ConsoleHelpers.AskYesNoQuestion("Would you like to add another property?", true);
            }

            //The new type is complete, return to the caller
            return type;
        }

        /// <summary>
        ///     Allows the user to create an SDSTypeProperty
        /// </summary>
        /// <returns></returns>
        private SdsTypeProperty GetTypeProperty()
        {
            //Guide the user through the steps to create a new property
            var property = new SdsTypeProperty();
            Console.WriteLine("Create property".ToAnsiBold());
            property.Id = ConsoleHelpers.AskQuestion("ID", "NewProperty");
            property.Name = ConsoleHelpers.AskQuestion("Name", property.Id);
            property.Description = ConsoleHelpers.AskQuestion("Description", $"{property.Name} description");
            property.InterpolationMode = ConsoleHelpers.AskOptions("Interpolation mode", SdsInterpolationMode.Default);
            property.IsKey = ConsoleHelpers.AskYesNoQuestion("Key");

            var typeCode = ConsoleHelpers.AskOptions("Type", SdsTypeCode.Double);

            property.SdsType = new SdsType {SdsTypeCode = typeCode};

            return property;
        }
    }
}