// // // <copyright file="EdsConsoleTools.cs" company="OSIsoft, LLC">
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
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSIsoft.Data;
using OSIsoft.Samples.Eds.ConsoleTool.AnsiSupport;
using OSIsoft.Samples.Eds.ConsoleTool.Utilities;
using OSIsoft.Samples.Eds.ConsoleTool.Wizards;

namespace OSIsoft.Samples.Eds.ConsoleTool
{
    /// <summary>
    ///     Main class for Eds Console Tools. Contains the main menu and will delegate actions to wizards.
    /// </summary>
    public class EdsConsoleTools
    {
        public static SdsService SdsService { get; private set; }
        public static ISdsDataService SdsDataService { get; private set; }
        public static ISdsMetadataService SdsMetadataService { get; private set; }


        public async Task Run()
        {
            Console.WriteLine("Welcome to [b]EDS Tools[/b]".ToAnsi());
            Console.WriteLine("First, we need to make sure we can connect to your EDS system");
            Console.WriteLine();

            //There are some default values for a typical EDS install. These can be found as constants in the DefaultValues class
            var edsUrl = DefaultValues.DefaultEdsUrl;
            var tenantId = DefaultValues.DefaultTenantId;
            var nameSpace = DefaultValues.DefaultNamespace;

            //Let's see if the default values make sense to the user, otherwise they will have the opportunity to correct them.
            Console.WriteLine(
                $"The default URL is [i]'{edsUrl}'[/i] with tenantid [i]'{tenantId}'[/i] and namespace [i]'{nameSpace}'[/i]."
                    .ToAnsi());
            if (!ConsoleHelpers.AskYesNoQuestion("Does this look correct?", true))
            {
                edsUrl = ConsoleHelpers.AskQuestion("URL?", edsUrl);
                tenantId = ConsoleHelpers.AskQuestion("Tenant ID?", tenantId);
                nameSpace = ConsoleHelpers.AskQuestion("Namespace?", nameSpace);
            }

            //With the correct URL, we can instantiate our SdsService
            SdsService = new SdsService(new Uri(edsUrl));
            //Instantiate the SDS Data Service and SDS Meta Data Service as well, so they can be used by the wizards.
            SdsDataService = SdsService.GetDataService(tenantId, nameSpace);
            SdsMetadataService = SdsService.GetMetadataService(tenantId, nameSpace);

            //We are now ready to show the main menu
            await this.ShowMainMenu();
        }

        private async Task ShowMainMenu()
        {
            //Create a list of menu options
            var options = new List<string>
            {
                "Create Type",
                "Create Stream",
                "List Types",
                "List Streams",
                "Exit EDS Tools"
            };

            //We put the main menu in a loop, so that when a user has used an option they will return to the main menu
            while (true)
            {
                //Ask the user what they want to do. This function returns a tuple with the index of the choice, and the text.
                var choice = ConsoleHelpers.AskOptions("What would you like to do?", options);
                switch (choice.index)
                {
                    //Create Type
                    case 0:
                    {
                        this.CreateType();

                        break;
                    }
                    //Create Stream
                    case 1:
                    {
                        await this.CreateStream();
                        break;
                    }
                    //List types
                    case 2:
                    {
                        this.ListTypes();
                        break;
                    }
                    case 3:
                    {
                        this.ListStreams();
                        break;
                    }
                    //Default is exit
                    default:
                    {
                        this.Exit();
                        break;
                    }
                }
            }
        }

        private void ListTypes()
        {
            var wizard = new TypeListWizard();
            wizard.Run(SdsMetadataService);
        }

        private void ListStreams()
        {
            var wizard = new StreamListWizard();
            wizard.Run(SdsMetadataService);
        }

        private async Task CreateStream()
        {
            //Run the Create Stream Wizard to get the SDS Stream
            var wizard = new StreamCreatorWizard();
            var stream = await wizard.Create(SdsMetadataService);

            Console.WriteLine("Stream summary:".ToAnsiBold());
            Console.WriteLine();
            Console.WriteLine("ID".PadRight(20) + stream.Id);
            Console.WriteLine("Name".PadRight(20) + stream.Name);
            Console.WriteLine("Type".PadRight(20) + stream.TypeId);

            Console.WriteLine();
            var create = ConsoleHelpers.AskYesNoQuestion("Are you sure you want to create this stream", true);
            Console.WriteLine();
            if (create)
            {
                ConsoleHelpers.ExecuteWhileSpinning(async () =>
                    await SdsMetadataService.GetOrCreateStreamAsync(stream));
                Console.WriteLine("Stream created");

                if (ConsoleHelpers.AskYesNoQuestion("Do you want to view the new stream?", true))
                {
                    //Output the new type to the console by serializing it to json and highlighting the json syntax
                    var json = JsonConvert.SerializeObject(stream, Formatting.Indented);
                    Console.WriteLine(json.SyntaxHighlightJson());
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }
        }


        private void CreateType()
        {
            //Run the Type Creator wizard first to get the SDS Type. 
            var wizard = new TypeCreatorWizard();
            var newType = wizard.Run();

            //Display the newly created type to the user
            Console.WriteLine("Type summary:".ToAnsiBold());
            Console.WriteLine();
            Console.WriteLine("ID".PadRight(20) + newType.Id);
            Console.WriteLine("Name".PadRight(20) + newType.Name);
            Console.WriteLine("Type".PadRight(20) + newType.SdsTypeCode);
            Console.WriteLine("Interpolation mode".PadRight(20) + newType.InterpolationMode);
            Console.WriteLine("Extrapolation mode".PadRight(20) + newType.ExtrapolationMode);
            Console.WriteLine();
            Console.WriteLine("Properties".ToAnsiUnderline());

            foreach (var prop in newType.Properties)
                Console.WriteLine($"{prop.Name} " + $"[{prop.SdsType.SdsTypeCode}]".ToAnsiDebug() +
                                  (prop.IsKey ? " [Key]".ToAnsiSuccess() : ""));

            //Ask for confirmation to create the type
            Console.WriteLine();
            var create = ConsoleHelpers.AskYesNoQuestion("Are you sure you want to create this type", true);
            Console.WriteLine();


            if (create)
            {
                //Call the SDS MetaData service to create the type. We do this while a 'spinner' is spinning to indicate the system is busy
                ConsoleHelpers.ExecuteWhileSpinning(async () => await SdsMetadataService.GetOrCreateTypeAsync(newType));

                Console.WriteLine("Type created");
                if (ConsoleHelpers.AskYesNoQuestion("Do you want to view the new type?", true))
                {
                    //Output the new type to the console by serializing it to json and highlighting the json syntax
                    var json = JsonConvert.SerializeObject(newType, Formatting.Indented);
                    Console.WriteLine(json.SyntaxHighlightJson());
                    Console.WriteLine("Press any key to continue");
                    Console.Read();
                }
            }

            //We will now return to the main menu
        }

        private void Exit()
        {
            //Exit gracefully
            Console.WriteLine("Thank you for using EDS Tools");
            Environment.Exit(0);
        }
    }
}