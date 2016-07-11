using System.Collections.Generic;
using System.IO;

namespace TemplateGetter.Io
{
    public class IoHandler : IIoHandler
    {
        public string[] GetFilesFromTemplateDirectory(string pathToTemplateFiles)
        {
            var fileArray = Directory.GetFiles(pathToTemplateFiles);
            return fileArray;
        }
    }
}