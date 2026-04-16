using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Core.Ubl.EnvelopeSerialization
{
    [GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
    [XmlRoot(Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader", IsNullable = false)]
    public partial class StandardBusinessDocument
    {
        private StandardBusinessDocumentHeader standardBusinessDocumentHeaderField;
        private XmlElement anyField;

        public StandardBusinessDocumentHeader StandardBusinessDocumentHeader
        {
            get => standardBusinessDocumentHeaderField;
            set => standardBusinessDocumentHeaderField = value;
        }

        [XmlAnyElement]
        public XmlElement Any
        {
            get => anyField;
            set => anyField = value;
        }
    }
}
