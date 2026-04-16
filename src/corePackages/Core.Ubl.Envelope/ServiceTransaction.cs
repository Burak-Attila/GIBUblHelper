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
    public partial class ServiceTransaction
    {
        private TypeOfServiceTransaction typeOfServiceTransactionField;
        private bool typeOfServiceTransactionFieldSpecified;
        private string isNonRepudiationRequiredField;
        private string isAuthenticationRequiredField;
        private string isNonRepudiationOfReceiptRequiredField;
        private string isIntelligibleCheckRequiredField;
        private string isApplicationErrorResponseRequestedField;
        private string timeToAcknowledgeReceiptField;
        private string timeToAcknowledgeAcceptanceField;
        private string timeToPerformField;
        private string recurrenceField;

        [XmlAttribute]
        public TypeOfServiceTransaction TypeOfServiceTransaction
        {
            get => typeOfServiceTransactionField;
            set => typeOfServiceTransactionField = value;
        }

        [XmlIgnore]
        public bool TypeOfServiceTransactionSpecified
        {
            get => typeOfServiceTransactionFieldSpecified;
            set => typeOfServiceTransactionFieldSpecified = value;
        }

        [XmlAttribute]
        public string IsNonRepudiationRequired
        {
            get => isNonRepudiationRequiredField;
            set => isNonRepudiationRequiredField = value;
        }

        [XmlAttribute]
        public string IsAuthenticationRequired
        {
            get => isAuthenticationRequiredField;
            set => isAuthenticationRequiredField = value;
        }

        [XmlAttribute]
        public string IsNonRepudiationOfReceiptRequired
        {
            get => isNonRepudiationOfReceiptRequiredField;
            set => isNonRepudiationOfReceiptRequiredField = value;
        }

        [XmlAttribute]
        public string IsIntelligibleCheckRequired
        {
            get => isIntelligibleCheckRequiredField;
            set => isIntelligibleCheckRequiredField = value;
        }

        [XmlAttribute]
        public string IsApplicationErrorResponseRequested
        {
            get => isApplicationErrorResponseRequestedField;
            set => isApplicationErrorResponseRequestedField = value;
        }

        [XmlAttribute]
        public string TimeToAcknowledgeReceipt
        {
            get => timeToAcknowledgeReceiptField;
            set => timeToAcknowledgeReceiptField = value;
        }

        [XmlAttribute]
        public string TimeToAcknowledgeAcceptance
        {
            get => timeToAcknowledgeAcceptanceField;
            set => timeToAcknowledgeAcceptanceField = value;
        }

        [XmlAttribute]
        public string TimeToPerform
        {
            get => timeToPerformField;
            set => timeToPerformField = value;
        }

        [XmlAttribute]
        public string Recurrence
        {
            get => recurrenceField;
            set => recurrenceField = value;
        }
    }
}
