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
    public partial class Partner
    {
        private PartnerIdentification identifierField;
        private ContactInformation[] contactInformationField;

        public PartnerIdentification Identifier
        {
            get => identifierField;
            set => identifierField = value;
        }

        [XmlElement("ContactInformation")]
        public ContactInformation[] ContactInformation
        {
            get => contactInformationField;
            set => contactInformationField = value;
        }
    }
}
