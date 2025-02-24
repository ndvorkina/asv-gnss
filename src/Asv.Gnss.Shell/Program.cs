﻿using System;
using Spectre.Console.Cli;


namespace Asv.Gnss.Shell
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<PrintBytesCommand>("print");
                config.AddCommand<UbxCommand>("ubx");
#if DEBUG
                config.PropagateExceptions();
                config.ValidateExamples();
#endif
            });
            return app.Run(args);
        }
    }
}
