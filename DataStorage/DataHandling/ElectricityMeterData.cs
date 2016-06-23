using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DataStorage.DataHandling
{
    public class ElectricityMeterData
    {
        public int Id { get; set; }
        public int ElectricityValue { get; set; }

        public byte[] ImageOfFrame { get; set; }

        public byte[] ImageOfFrameWithOutlines { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool HasAcceptedElectricityValue { get; set; }
    }
}