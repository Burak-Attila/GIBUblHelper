using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Ubl.EnvelopeSerialization
{
    [GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
    [XmlRoot(Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", IsNullable = false)]
    public partial class StandardBusinessDocumentHeader
    {
        private string headerVersionField;
        private Partner[] senderField;
        private Partner[] receiverField;
        private DocumentIdentification documentIdentificationField;
        private Manifest manifestField;
        private Scope[] businessScopeField;

        public string HeaderVersion
        {
            get => headerVersionField;
            set => headerVersionField = value;
        }

        [XmlElement("Sender")]
        public Partner[] Sender
        {
            get => senderField;
            set => senderField = value;
        }

        [XmlElement("Receiver")]
        public Partner[] Receiver
        {
            get => receiverField;
            set => receiverField = value;
        }

        public DocumentIdentification DocumentIdentification
        {
            get => documentIdentificationField;
            set => documentIdentificationField = value;
        }

        public Manifest Manifest
        {
            get => manifestField;
            set => manifestField = value;
        }

        [XmlArrayItem(IsNullable = false)]
        public Scope[] BusinessScope
        {
            get => businessScopeField;
            set => businessScopeField = value;
        }
    }
}
