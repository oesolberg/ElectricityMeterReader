using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using ElMeInterfaces;

namespace TemplateGetter.Template
{
    public class TemplateDataCreator : ITemplateDataCreator
    {
        public List<ITemplateData> CreateTemplates(string[] fileArray)
        {
            var templateDataList=new List<ITemplateData>();

            foreach (var filepath in fileArray)
            {
                if (!FilepathIsAnAcceptedFile(filepath)) continue;

                var templateData = ConvertFilepathToTemplateData(filepath);
                if(templateData!=null && templateData.NumberValue>-1)
                    templateDataList.Add(templateData);
            }
            return templateDataList;
        }

        private ITemplateData ConvertFilepathToTemplateData(string filepath)
        {
            var templateData=new TemplateData();
            templateData.TemplatePath = filepath;
            templateData.NumberValue = GetNumberValueFromFilepath(filepath);
            return templateData;
        }

        private int GetNumberValueFromFilepath(string filepath)
        {
            var filename = Path.GetFileNameWithoutExtension(filepath);

            if (string.IsNullOrWhiteSpace(filename)) return -1;

            var firstDigitAsString = filename.Substring(0, 1);

            if (string.IsNullOrWhiteSpace(firstDigitAsString)) return -1;
                
            return Int32.Parse(firstDigitAsString);
     
        }

        private bool FilepathIsAnAcceptedFile(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath)) return false;
            if (!FilenameIsCorrectFormat(filepath)) return false;
            return true;
        }

        private bool FilenameIsCorrectFormat(string filepath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filepath);

            if (string.IsNullOrWhiteSpace(fileName)) return false;
            if (!fileName.Contains("_")) return false;
            if (fileName.IndexOf("_") != 1) return false;
            if (!FirstCharacterIsDigit(fileName)) return false;

            return true;
        }

        private bool FirstCharacterIsDigit(string fileName)
        {
            return Char.IsDigit(fileName, 0);
        }
    }
}