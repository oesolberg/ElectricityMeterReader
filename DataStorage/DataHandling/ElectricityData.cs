using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DataStorage.DataHandling
{
    public class ElectricityData
    {
        public int Id { get; set; }
        public int ElectricityValue { get; set; }
        public int? ElectricityValueSetByUser { get; set; }

        public string OriginalFilename { get; set; }

        public byte[] JpgImageOfFrame { get; set; }

        public byte[] JpgImageOfFrameWithOutlines { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime FileCreatedDateTime { get; set; }

        public DateTime? ChangedDateTime { get; set; }
        public bool HasAcceptedElectricityValue { get; set; }
    }
}