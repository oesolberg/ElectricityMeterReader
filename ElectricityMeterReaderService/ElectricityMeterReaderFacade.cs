using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ElectricityMeterReaderService.ImageHandlingLogic;

namespace ElectricityMeterReaderService
{
    public partial class ElectricityMeterReaderFacade : ServiceBase
    {
        private int _sleepInterval;

        public ElectricityMeterReaderFacade()
        {
            InitializeComponent();
            //fileSystemWatcher.Changed += fileSystemWatcher_Changed;
            fileSystemWatcher.Created += fileSystemWatcher_Changed;



        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher.Filter = ConfigurationManager.AppSettings["filter"];
            fileSystemWatcher.Path = ConfigurationManager.AppSettings["folder"];
            _sleepInterval = 10000;
            var sleepIntervalFromConfig = ConfigurationManager.AppSettings["sleepinterval"];
            int sleepIntervalFromConfigConvertedToInt;
            if (!string.IsNullOrWhiteSpace(sleepIntervalFromConfig) && int.TryParse(sleepIntervalFromConfig, out sleepIntervalFromConfigConvertedToInt))
            {
                _sleepInterval = sleepIntervalFromConfigConvertedToInt;
            }

        }

        protected override void OnStop()
        {
        }

        private void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            //Some file has changed.
            Thread.Sleep(_sleepInterval); //Sleep 10 seconds to get the file when it is done saving
            if (e.ChangeType == WatcherChangeTypes.Created && FileExists(e.FullPath))
            {
                var stopWatch=new Stopwatch();
                stopWatch.Start();
                LogToConsoleIfPossible(e.FullPath);
                LogToConsoleIfPossible(e.ChangeType.ToString());

                var imageHandler = new ElectricityImageHandler(e.FullPath);
                var imageData=imageHandler.DoImageProcessing();
                stopWatch.Stop();
                
                LogToConsoleIfPossible("Done processing image in " + stopWatch.ElapsedMilliseconds.ToString("N")+" ms");
                if (imageData != null)
                {
                    LogToConsoleIfPossible("Number found: " + imageData.ImageNumber.ToString("000000"));
                }
                else
                {
                    LogToConsoleIfPossible("No data extracted");
                }
                    

            }
        }

        private bool FileExists(string fullPath)
        {
            return System.IO.File.Exists(fullPath);
        }

        private void LogToConsoleIfPossible(string stringToWrite)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine(stringToWrite);
            }
        }

    }
}
