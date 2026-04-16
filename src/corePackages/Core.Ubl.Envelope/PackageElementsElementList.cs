
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

using System.Xml;
using System.Xml.Serialization;

namespace Core.Ubl.EnvelopeSerialization;

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.efatura.gov.tr/package-namespace")]
public partial class PackageElementsElementList
{
    private XmlElement[] anyField;

    [XmlAnyElement]
    public XmlElement[] Any
    {
        get => anyField;
        set => anyField = value;
    }
}
