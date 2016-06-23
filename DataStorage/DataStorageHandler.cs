using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage.DataHandling;
using ElMeInterfaces;

namespace DataStorage
{
    public class DataStorageHandler
    {


        public void DoStorage(IImageData imageData)
        {
            //Get minimum value and check (config)
            if (IsImageDataValid(imageData))
            {
                var numberValidator = new NumberValidator();
                var imageStorageHandler = new StoreImageToDb();
                var isValid = numberValidator.IsElectricityNumberValid(imageData);
                imageStorageHandler.Execute(imageData, isValid);
            }

        }

        private bool IsImageDataValid(IImageData imageData)
        {
            if (imageData == null) return false;
            if (imageData.ImageNumber < 1) return false;
            return true;
        }
    }
}
