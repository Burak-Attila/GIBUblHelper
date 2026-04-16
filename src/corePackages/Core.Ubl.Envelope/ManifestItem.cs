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
    public partial class ManifestItem
    {
        private string mimeTypeQualifierCodeField;
        private string uniformResourceIdentifierField;
        private string descriptionField;
        private string languageCodeField;

        public string MimeTypeQualifierCode
        {
            get => mimeTypeQualifierCodeField;
            set => mimeTypeQualifierCodeField = value;
        }

        [XmlElement(DataType = "anyURI")]
        public string UniformResourceIdentifier
        {
            get => uniformResourceIdentifierField;
            set => uniformResourceIdentifierField = value;
        }

        public string Description
        {
            get => descriptionField;
            set => descriptionField = value;
        }

        public string LanguageCode
        {
            get => languageCodeField;
            set => languageCodeField = value;
        }
    }
}
