using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ElMeInterfaces;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class ElectricityImageHandler
    {
        private string _pathToFileToProcess;
        private readonly string _filename;
        

        public ElectricityImageHandler(string filePath)
        {
            _pathToFileToProcess = filePath;
            _filename = Path.GetFileName(filePath);
            
        }

        //Find numberplate template and do crop if found

        //Process cropped numberplate and find numbers

        public IImageData DoImageProcessing()
        {
            //Find imageframe
            
            var imageDataToReturn=new ImageData();
            var numberPlateImage = GetNumberPlateImage();
            if (numberPlateImage == null) return null;
            var numberPlateImageToUseForFinalImage = numberPlateImage.Clone();
            var numberPlateImageUntouched = numberPlateImage.Clone();
            //Find numbers and rectangles
            var foundDigitData = GetDigitData(numberPlateImage);
            if (foundDigitData == null) return null;
            //Produce final image and digit data. For now save the image with a modified name with the digits.
            var finalImage = GetFinalImage(numberPlateImageToUseForFinalImage, foundDigitData);

            var electricityMeterNumber = foundDigitData.GetFoundNumber();

            imageDataToReturn.ImageCropped = numberPlateImageUntouched;
            imageDataToReturn.ImageNumber = electricityMeterNumber;
            imageDataToReturn.ImageWithDigitsOutlined = finalImage;
            imageDataToReturn.OriginalFilename = _filename;
            imageDataToReturn.CreatedDateTime = GetCreatedDateTimeFromFile();
            //For now save the image with the electricity number as a part of the filename
            SaveImageToTempFolder(finalImage, electricityMeterNumber, _pathToFileToProcess);
            return imageDataToReturn;
        }

        private DateTime GetCreatedDateTimeFromFile()
        {
            return File.GetCreationTime(_pathToFileToProcess);
        }

        private static Image<Bgr, byte> GetFinalImage(Image<Bgr, byte> numberPlateImage, DigitsAndDigitRectangles foundDigitData)
        {
            var finalImageCreator = new FinalImageCreator(numberPlateImage, foundDigitData);
            var finalImage = finalImageCreator.CreateImageWithDigitsOutlined();
            return finalImage;
        }

        private static DigitsAndDigitRectangles GetDigitData(Image<Bgr, byte> numberPlateImage)
        {
            var digitDataExtractor = new GetDigitsFromCroppedImage();
            var foundDigitData = digitDataExtractor.Process(numberPlateImage);
            return foundDigitData;
        }

        private Image<Bgr, byte> GetNumberPlateImage()
        {
            var numberPlateExtractor = new GetNumberPlateFromImage(_pathToFileToProcess);
            var numberPlateImage = numberPlateExtractor.Process();
            return numberPlateImage;
        }

        private void SaveImageToTempFolder(Image<Bgr, byte> finalImage, double electricityMeterNumber, string orignialFilenameFullPath)
        {
            var saveFolder = @"c:\temp\electricityNumbers";
            if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
            //Create filename <original filename>_<electricitynumber>.jpg

            var originalFilename = Path.GetFileNameWithoutExtension(orignialFilenameFullPath);
            var newFileName = originalFilename + "_" + electricityMeterNumber.ToString("000000") + ".jpg";

            SaveImageWithNewName(finalImage, saveFolder, newFileName);


        }

        private void SaveImageWithNewName(Image<Bgr, byte> finalImage, string saveFolder, string newFileName)
        {
            SaveJpeg(Path.Combine(saveFolder,newFileName),finalImage.ToBitmap(),100);
        }

        private void SaveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = this.GetEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

    }
}