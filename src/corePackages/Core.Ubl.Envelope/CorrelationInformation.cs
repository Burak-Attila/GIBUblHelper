
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
    public partial class CorrelationInformation
    {
        private DateTime requestingDocumentCreationDateTimeField;
        private bool requestingDocumentCreationDateTimeFieldSpecified;
        private string requestingDocumentInstanceIdentifierField;
        private DateTime expectedResponseDateTimeField;
        private bool expectedResponseDateTimeFieldSpecified;

        public DateTime RequestingDocumentCreationDateTime
        {
            get => requestingDocumentCreationDateTimeField;
            set => requestingDocumentCreationDateTimeField = value;
        }

        [XmlIgnore]
        public bool RequestingDocumentCreationDateTimeSpecified
        {
            get => requestingDocumentCreationDateTimeFieldSpecified;
            set => requestingDocumentCreationDateTimeFieldSpecified = value;
        }

        public string RequestingDocumentInstanceIdentifier
        {
            get => requestingDocumentInstanceIdentifierField;
            set => requestingDocumentInstanceIdentifierField = value;
        }

        public DateTime ExpectedResponseDateTime
        {
            get => expectedResponseDateTimeField;
            set => expectedResponseDateTimeField = value;
        }

        [XmlIgnore]
        public bool ExpectedResponseDateTimeSpecified
        {
            get => expectedResponseDateTimeFieldSpecified;
            set => expectedResponseDateTimeFieldSpecified = value;
        }
    }
}
