
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
    [XmlRoot(Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", IsNullable = false)]
    public partial class BusinessService
    {
        private string businessServiceNameField;
        private ServiceTransaction serviceTransactionField;

        public string BusinessServiceName
        {
            get => businessServiceNameField;
            set => businessServiceNameField = value;
        }

        public ServiceTransaction ServiceTransaction
        {
            get => serviceTransactionField;
            set => serviceTransactionField = value;
        }
    }
}
