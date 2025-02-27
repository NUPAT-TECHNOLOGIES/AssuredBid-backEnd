using Newtonsoft.Json;

namespace AssuredBid.Models
{
    public class CompanyProfile
    {
        public Accounts accounts { get; set; }
        public bool can_file { get; set; }
        public string company_name { get; set; }
        public string company_number { get; set; }
        public string company_status { get; set; }
        public ConfirmationStatement confirmation_statement { get; set; }
        public string date_of_creation { get; set; }
        public string etag { get; set; }
        public bool has_charges { get; set; }
        public bool has_insolvency_history { get; set; }
        public string jurisdiction { get; set; }
        public Links links { get; set; }
        public RegisteredOfficeAddress registered_office_address { get; set; }
        public bool registered_office_is_in_dispute { get; set; }
        public List<string> sic_codes { get; set; }
        public string type { get; set; }
        public bool undeliverable_registered_office_address { get; set; }
        public bool has_super_secure_pscs { get; set; }
    }

    public class AccountingReferenceDate
    {
        public string day { get; set; }
        public string month { get; set; }
    }

    public class Accounts
    {
        public AccountingReferenceDate accounting_reference_date { get; set; }
        public NextAccounts next_accounts { get; set; }
        public string next_due { get; set; }
        public string next_made_up_to { get; set; }
        public bool overdue { get; set; }
    }

    public class ConfirmationStatement
    {
        public string next_due { get; set; }
        public string next_made_up_to { get; set; }
        public bool overdue { get; set; }
    }

    public class Links
    {
        public string persons_with_significant_control { get; set; }
        public string self { get; set; }
        public string filing_history { get; set; }
        public string officers { get; set; }
    }

    public class NextAccounts
    {
        public string due_on { get; set; }
        public bool overdue { get; set; }
        public string period_end_on { get; set; }
        public string period_start_on { get; set; }
    }

    public class RegisteredOfficeAddress
    {
        public string address_line_1 { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string postal_code { get; set; }
    }

}
