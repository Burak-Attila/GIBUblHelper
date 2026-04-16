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
    [XmlRoot(Namespace = "http://www.efatura.gov.tr/package-namespace", IsNullable = false)]
    public partial class Package
    {
        private PackageElements[] elementsField;

        [XmlElement("Elements", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PackageElements[] Elements
        {
            get => elementsField;
            set => elementsField = value;
        }
    }
}
