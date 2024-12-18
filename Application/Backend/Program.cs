﻿using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Autofac;

using Backend.Configs;
using Backend.Queue;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Backend
{
    public class Program
    {

        private static IApplicationManager? _manager;
        private static ILogger? _logger;

        public static async Task Main(string[] argv)
        {
            NLogLoggerFactory loggerFactory = new NLogLoggerFactory();
            _logger = loggerFactory.CreateLogger(nameof(Program));

            try
            {
                IContainer container = AutofacConfig.GetDIContainer();
                _manager = container.Resolve<IApplicationManager>();

                await _manager!.Run();

            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, ex.Message);
            }

            while (true)
            {
                Console.WriteLine("ApplicationManager: press Esc to exit.");

                int key = Console.Read();
                if (key == (int)'Z')
                    break;
            }
        }
    }
}

