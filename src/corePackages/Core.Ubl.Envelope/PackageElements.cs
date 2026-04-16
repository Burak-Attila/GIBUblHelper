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
    [XmlType(AnonymousType = true, Namespace = "http://www.efatura.gov.tr/package-namespace")]
    public partial class PackageElements
    {
        private string elementTypeField;
        private int elementCountField;
        private PackageElementsElementList elementListField;

        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ElementType
        {
            get => elementTypeField;
            set => elementTypeField = value;
        }

        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int ElementCount
        {
            get => elementCountField;
            set => elementCountField = value;
        }

        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PackageElementsElementList ElementList
        {
            get => elementListField;
            set => elementListField = value;
        }
    }
}
