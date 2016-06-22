using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class GetNumberPlateFromImage
    {
        private readonly string _filepathToImage;
        private readonly string _filepathToTemplate;

        public GetNumberPlateFromImage()
        {
            
        }

        public GetNumberPlateFromImage(string filepathToImage)
        {
            _filepathToImage = filepathToImage;
            var templateFilename = ConfigurationManager.AppSettings["numberplatetemplate"];
            
            _filepathToTemplate = Path.Combine(ImageHandlingLogicHelper.GetTemplatePath(), templateFilename);
        }

        public Image<Bgr,byte> Process()
        {
        

            Image<Bgr, byte> source = new Image<Bgr, byte>(_filepathToImage);
            Image<Bgr, byte> template = new Image<Bgr, byte>(_filepathToTemplate);

            Image<Bgr, byte> imageToReturn = null;


            using (Image<Gray, float> result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.Ccoeff))
            {
                //Image<Gray, float> resultImage = result.Mul(resultMask.Pow(-1));
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                if (maxValues[0] > 0.9)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    var smallImage = CreateSmallImage(match, source, template.Size);

                    //Try to export the inside image without the borders
                    smallImage.ROI = new Rectangle(24, 22, 300, 54);
                    imageToReturn = smallImage.Copy();
                }
            }
            return imageToReturn;
        }

        private Image<Bgr, byte> CreateSmallImage(Rectangle match, Image<Bgr, byte> imageToShow, Size size)
        {
            imageToShow.ROI = match;
            return imageToShow.Copy();
        }


        //private void SaveJpeg(string path, Bitmap img, long quality)
        //{
        //    // Encoder parameter for image quality

        //    EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        //    // Jpeg image codec
        //    ImageCodecInfo jpegCodec = this.GetEncoderInfo("image/jpeg");

        //    if (jpegCodec == null)
        //        return;

        //    EncoderParameters encoderParams = new EncoderParameters(1);
        //    encoderParams.Param[0] = qualityParam;

        //    img.Save(path, jpegCodec, encoderParams);
        //}

        //private ImageCodecInfo GetEncoderInfo(string mimeType)
        //{
        //    // Get image codecs for all image formats
        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        //    // Find the correct image codec
        //    for (int i = 0; i < codecs.Length; i++)
        //        if (codecs[i].MimeType == mimeType)
        //            return codecs[i];
        //    return null;
        //}

    }
}