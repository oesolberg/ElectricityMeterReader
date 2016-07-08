using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElectricityMeterReaderService.ScanFolder
{
    public class FetchFilesFromWatchedFolder
    {
        private readonly string _folderToWatch;
        private readonly string _filefilter;

        public FetchFilesFromWatchedFolder(string folderToWatch, string filefilter)
        {
            _folderToWatch = folderToWatch;
            _filefilter = filefilter;
            
        }

        public List<string> Execute()
        {
            return Directory.GetFiles(_folderToWatch, _filefilter, SearchOption.AllDirectories).ToList();

            
        }
    }
}