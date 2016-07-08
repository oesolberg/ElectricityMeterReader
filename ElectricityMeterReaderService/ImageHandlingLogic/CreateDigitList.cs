using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class CreateDigitList
    {
        public List<DigitTemplateInfo> Execute(string fileSuffix = "")
        {
            return CreateDigitTemplateInfoList(fileSuffix);
        }
        private List<DigitTemplateInfo> CreateDigitTemplateInfoList(string fileSuffix)
        {
            var templatePath = ImageHandlingLogicHelper.GetTemplatePath();
            var digitTemplateInfoList = new List<DigitTemplateInfo>(10);
            for (int i = 0; i < 10; i++)
            {
                digitTemplateInfoList.Add(CreateDigitTemplateInfoList(i, templatePath, fileSuffix));
            }
            return digitTemplateInfoList;
        }

        private DigitTemplateInfo CreateDigitTemplateInfoList(int i, string templatePath,string fileSuffix)
        {
            var templateFileName = GetTemplateFileName(i,fileSuffix);

            return new DigitTemplateInfo() { DigitNumber = i, TemplateFilePath = Path.Combine(templatePath, templateFileName) };
        }



        private string GetTemplateFileName(int i,string fileSuffix)
        {
            return i.ToString()+fileSuffix+".jpg";
        }
    }
}