using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElMeInterfaces;

namespace TemplateGetter.FileSorting
{
    public class FileArraySorter : IFileArraySorter
    {
        private readonly string[] _allowedFileExtensionsArray;
        public FileArraySorter()
        {
            _allowedFileExtensionsArray =new string[] { "jpg","jpeg","png"};
        }
        public string[] GetImageFilepaths(string[] fileArray)
        {
            var fileList= GetAllFilepathsWithImageExtensions(fileArray);
            return fileArray.ToArray();
        }

        private List<string> GetAllFilepathsWithImageExtensions(string[] fileArray)
        {
            var fileList=new List<string>();
            foreach (var filepath in fileArray)
            {
                if (_allowedFileExtensionsArray.Contains(Path.GetExtension(filepath)))
                {
                   fileList.Add(filepath); 
                } 

            }
            return fileList;
        }
    }
}