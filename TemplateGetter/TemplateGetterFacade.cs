using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElMeInterfaces;
using TemplateGetter.FileSorting;
using TemplateGetter.Io;
using TemplateGetter.Template;

namespace TemplateGetter
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
    public class TemplateGetterFacade
    {
        private readonly string _pathToTemplates;
        private readonly IIoHandler _ioHandler;
        private readonly IFileArraySorter _fileArraySorter;
        private readonly ITemplateDataCreator _templateDataCreator;

        public TemplateGetterFacade(string pathToTemplates)
        {
            _pathToTemplates = pathToTemplates;
            _ioHandler= new IoHandler();
            _fileArraySorter = new FileArraySorter();
            _templateDataCreator=new TemplateDataCreator();
        }

        public List<ITemplateData> GetTemplateDataAsList()
        {
            //Open folder and get all files inside.
            //Get every file with .jpg extension and filename beginning with a number
            //Extract number value by digit before underscore

            //Get all files in the specified path
            var fileList = _ioHandler.GetFilesFromTemplateDirectory(_pathToTemplates);
            //Process the filepaths by sorting and only keeping the ones that are image files
            fileList = _fileArraySorter.GetImageFilepaths(fileList);
            //Create TemplateData for the files that are within format <digit>_<something>.<imageextension>
            var tempateDataList = _templateDataCreator.CreateTemplates(fileList);
            return tempateDataList;
        }
    }
}
