
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

using System.Xml.Serialization;

namespace Core.Ubl.EnvelopeSerialization
{
    [GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
    public partial class PartnerIdentification
    {
        private string authorityField;
        private string valueField;

        [XmlAttribute]
        public string Authority
        {
            get => authorityField;
            set => authorityField = value;
        }

        [XmlText]
        public string Value
        {
            get => valueField;
            set => valueField = value;
        }
    }
}
