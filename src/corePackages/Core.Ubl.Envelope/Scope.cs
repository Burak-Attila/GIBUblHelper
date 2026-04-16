
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
    public partial class Scope
    {
        private string typeField;
        private string instanceIdentifierField;
        private string identifierField;
        private object[] itemsField;

        public string Type
        {
            get => typeField;
            set => typeField = value;
        }

        public string InstanceIdentifier
        {
            get => instanceIdentifierField;
            set => instanceIdentifierField = value;
        }

        public string Identifier
        {
            get => identifierField;
            set => identifierField = value;
        }

        [XmlElement("BusinessService", typeof(BusinessService))]
        [XmlElement("CorrelationInformation", typeof(CorrelationInformation))]
        public object[] Items
        {
            get => itemsField;
            set => itemsField = value;
        }
    }
}
