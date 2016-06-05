using System;
using System.Collections.Generic;

namespace Rota.PayrollInterface.Models
{
    public class PayrollInterfacePerson
    {
        public string ClientId { get; set; }

        public string PersonalNumber { get; set; }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        public string Nationality { get; set; }

        public string PostCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string SocialInsuranceNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FamilyStatus { get; set; }

        public string MailAddress { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankNumber { get; set; }

        public string BankName { get; set; }

        public string BankCountry { get; set; }

        public string IBAN { get; set; }

        public string BIC { get; set; }

        public int AmountOfChildren { get; set; }

        public bool SingleParent { get; set; }

        public string Title { get; set; }

        public DateTime StartOfVacationYear { get; set; }

        public DateTime FirstEntrance { get; set; } //TOD evtl. anderer name

        public DateTime EndOfWorkPermittion { get; set; }

        public PayrollInterfaceContract Contract { get; set; }

        public IEnumerable<PayrollInterfaceContract> FormerContracts { get; set; }

        public bool Inactive { get; set; }

        public PayrollInterfacePerson() {
            Contract = new PayrollInterfaceContract();
            FormerContracts = new List<PayrollInterfaceContract>();
        }
    }
}