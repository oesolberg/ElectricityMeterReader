using System.Collections.Generic;
using ElMeInterfaces;

namespace TemplateGetter.Template
{
    public interface ITemplateDataCreator
    {
        List<ITemplateData> CreateTemplates(string[] fileArray);
    }
}