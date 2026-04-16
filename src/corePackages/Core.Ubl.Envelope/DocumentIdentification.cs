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
    public partial class DocumentIdentification
    {
        private string standardField;
        private string typeVersionField;
        private string instanceIdentifierField;
        private string typeField;
        private bool multipleTypeField;
        private bool multipleTypeFieldSpecified;
        private DateTime creationDateAndTimeField;

        public string Standard
        {
            get => standardField;
            set => standardField = value;
        }

        public string TypeVersion
        {
            get => typeVersionField;
            set => typeVersionField = value;
        }

        public string InstanceIdentifier
        {
            get => instanceIdentifierField;
            set => instanceIdentifierField = value;
        }

        public string Type
        {
            get => typeField;
            set => typeField = value;
        }

        public bool MultipleType
        {
            get => multipleTypeField;
            set => multipleTypeField = value;
        }

        [XmlIgnore]
        public bool MultipleTypeSpecified
        {
            get => multipleTypeFieldSpecified;
            set => multipleTypeFieldSpecified = value;
        }

        public DateTime CreationDateAndTime
        {
            get => creationDateAndTimeField;
            set => creationDateAndTimeField = value;
        }
    }
}
