﻿using System;
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
        private string _filename;


        public ElectricityImageHandler()
        {

        }

        //Find numberplate template and do crop if found

        //Process cropped numberplate and find numbers

        public IImageData DoImageProcessing(string filePath)
        {

            //Need to refactor

            /*
             * 1. Objects for loading all the different template images. Should handle that we do not have all for the different placements (100.000, 10.000. 1.000, 100, 10,1
             * 2. Check for matches doing the following. 
             *      Trying to get the numberplate. 
             *      Check found numberplate
             *      If no numberplate found, check the whole image
             * Get all the different results and check how the rectangles match. We will have more than one hit on most of the numbers so a lot of them will overlap. Clean it up and get a number like xxx.xxx
            */
            _pathToFileToProcess = filePath;
            _filename = Path.GetFileName(filePath);

            //Find imageframe

            var imageDataToReturn = new ImageData();
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
            imageDataToReturn.FileCreatedDateTime = GetFileCreatedDateTime();
            //For now save the image with the electricity number as a part of the filename
            //SaveImageToTempFolder(finalImage, electricityMeterNumber, _pathToFileToProcess);
            return imageDataToReturn;
        }

        private DateTime GetFileCreatedDateTime()
        {
            return File.GetLastWriteTime(_pathToFileToProcess);
        }

        private static Image<Bgr, byte> GetFinalImage(Image<Bgr, byte> numberPlateImage, DigitsAndDigitRectangles foundDigitData)
        {
            var finalImageCreator = new FinalImageCreator(numberPlateImage, foundDigitData);
            var finalImage = finalImageCreator.CreateImageWithDigitsOutlined();
            return finalImage;
        }

        private static DigitsAndDigitRectangles GetDigitData(Image<Bgr, byte> numberPlateImage)
        {
            var digitListCreator = new CreateDigitList();
            var digitList = digitListCreator.Execute();
            var digitDataExtractor = new GetDigitsFromCroppedImage();
            var foundDigitData = digitDataExtractor.Process(numberPlateImage, digitList);
            if (foundDigitData == null)
            {
                digitList = digitListCreator.Execute("_new");
                foundDigitData = digitDataExtractor.Process(numberPlateImage, digitList);
                //try to find in new format
            }
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
            SaveJpeg(Path.Combine(saveFolder, newFileName), finalImage.ToBitmap(), 100);
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