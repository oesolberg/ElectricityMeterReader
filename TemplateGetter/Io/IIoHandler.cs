using System.Collections.Generic;
using System.IO;

namespace TemplateGetter.Io
{
    public interface IIoHandler
    {
        string[] GetFilesFromTemplateDirectory(string pathToTemplateFiles);

    }
}