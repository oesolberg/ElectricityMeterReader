using ElMeInterfaces;

namespace TemplateGetter
{
    public class TemplateData : ITemplateData
    {
        public int NumberValue{ get; set; }

        public string TemplatePath { get; set; }
    }
}