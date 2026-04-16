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
    public partial class Manifest
    {
        private string numberOfItemsField;
        private ManifestItem[] manifestItemField;

        [XmlElement(DataType = "integer")]
        public string NumberOfItems
        {
            get => numberOfItemsField;
            set => numberOfItemsField = value;
        }

        [XmlElement("ManifestItem")]
        public ManifestItem[] ManifestItem
        {
            get => manifestItemField;
            set => manifestItemField = value;
        }
    }
}
