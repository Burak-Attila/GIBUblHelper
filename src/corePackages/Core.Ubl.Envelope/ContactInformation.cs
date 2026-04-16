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
    public partial class ContactInformation
    {
        private string contactField;
        private string emailAddressField;
        private string faxNumberField;
        private string telephoneNumberField;
        private string contactTypeIdentifierField;

        public string Contact
        {
            get => contactField;
            set => contactField = value;
        }

        public string EmailAddress
        {
            get => emailAddressField;
            set => emailAddressField = value;
        }

        public string FaxNumber
        {
            get => faxNumberField;
            set => faxNumberField = value;
        }

        public string TelephoneNumber
        {
            get => telephoneNumberField;
            set => telephoneNumberField = value;
        }

        public string ContactTypeIdentifier
        {
            get => contactTypeIdentifierField;
            set => contactTypeIdentifierField = value;
        }
    }
}
