using System;

namespace ElectricityMeterReaderService.ScanFolder
{
    public class MissingFileData
    {
        public string Filepath { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}