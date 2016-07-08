using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage.DataHandling;
using ElMeInterfaces;

namespace DataStorage
{
    public class DataStorageHandler
    {
        private readonly NumberValidator _numberValidator;
        private readonly StoreImageToDb _imageStorageHandler;

        public DataStorageHandler()
        {
            _numberValidator = new NumberValidator();
            _imageStorageHandler = new StoreImageToDb();

        }

        public void DoStorage(IImageData imageData)
        {
            //Get minimum value and check (config)
            if (IsImageDataValid(imageData))
            {
                var isValid = _numberValidator.IsElectricityNumberValid(imageData);
                _imageStorageHandler.Execute(imageData, isValid);
            }

        }

        private bool IsImageDataValid(IImageData imageData)
        {
            if (imageData == null) return false;
            if (imageData.ImageNumber < 1) return false;
            return true;
        }



        public void StoreFileInBlackList(string filepath)
        {
            var originalFileName = Path.GetFileName(filepath);

            _imageStorageHandler.BlackListFile(originalFileName, filepath);
        }
    }
}
