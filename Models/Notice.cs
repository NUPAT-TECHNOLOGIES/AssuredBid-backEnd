namespace AssuredBid.Models
{
    public class Notice
    {
        public string? Uri { get; set; }
        public string? Version { get; set; }
        public List<string>? Extensions { get; set; }
        public string? PublishedDate { get; set; }
        public Publisher? Publisher { get; set; }
        public string? License { get; set; }
        public string? PublicationPolicy { get; set; }
        public List<Release>? Releases { get; set; }
    }

    public class Publisher
    {
        public string? Name { get; set; }
        public string? Scheme { get; set; }
        public string? Uid { get; set; }
        public string? Uri { get; set; }
    }

    public class Release
    {
        public string? Ocid { get; set; }
        public string? Id { get; set; }
        public string? Date { get; set; }
        public List<string>? Tag { get; set; }
        public string? Description { get; set; }
        public string? InitiationType { get; set; }
        public Tender? Tender { get; set; }
        public List<Party>? Parties { get; set; }
        public Buyer? Buyer { get; set; }
        public string? Language { get; set; }
    }

    public class Tender
    {
        public string? Id { get; set; }
        public LegalBasis? LegalBasis { get; set; }
        public string? Title { get; set; }
        public string? Status { get; set; }
        public Classification? Classification { get; set; }
        public string? MainProcurementCategory { get; set; }
        public string? Description { get; set; }
        public Value? Value { get; set; }
        public List<Lot>? Lots { get; set; }
        public List<Item>? Items { get; set; }
        public Communication? Communication { get; set; }
        public List<string>? SubmissionMethod { get; set; }
        public string? SubmissionMethodDetails { get; set; }
        public SelectionCriteria? SelectionCriteria { get; set; }
        public List<Document>? Documents { get; set; }
        public OtherRequirements? OtherRequirements { get; set; }
        public ContractTerms? ContractTerms { get; set; }
        public Techniques? Techniques { get; set; }
        public List<string>? CoveredBy { get; set; }
        public AwardPeriod? AwardPeriod { get; set; }
        public string? ReviewDetails { get; set; }
    }

    public class LegalBasis
    {
        public string? Id { get; set; }
        public string? Scheme { get; set; }
    }

    public class Classification
    {
        public string? Scheme { get; set; }
        public string? Id { get; set; }
        public string? Description { get; set; }
    }

    public class Value
    {
        public decimal? Amount { get; set; } // Updated to decimal
        public string? Currency { get; set; }
    }

    public class Lot
    {
        public string? Id { get; set; }
        public string? Description { get; set; }
        public AwardCriteria? AwardCriteria { get; set; }
        public Value? Value { get; set; } // Updated to use Value class
        public ContractPeriod? ContractPeriod { get; set; }
        public bool? HasRenewal { get; set; }
        public Renewal? Renewal { get; set; }
        public SubmissionTerms? SubmissionTerms { get; set; }
        public bool? HasOptions { get; set; }
        public Options? Options { get; set; }
        public string? Status { get; set; }
    }

    public class AwardCriteria
    {
        public List<Criteria>? Criteria { get; set; }
    }

    public class Criteria
    {
        public string? Type { get; set; }
        public string? Description { get; set; }
        public List<string>? AppliesTo { get; set; } // Correctly mapped to match JSON.
    }

    public class ContractPeriod
    {
        public int? DurationInDays { get; set; }
    }

    public class Renewal
    {
        public string? Description { get; set; }
    }

    public class SubmissionTerms
    {
        public string? VariantPolicy { get; set; }
    }

    public class Options
    {
        public string? Description { get; set; }
    }

    public class Item
    {
        public string? Id { get; set; }
        public List<AdditionalClassification>? AdditionalClassifications { get; set; }
        public List<DeliveryAddress>? DeliveryAddresses { get; set; }
        public string? RelatedLot { get; set; }
    }

    public class AdditionalClassification
    {
        public string? Scheme { get; set; }
        public string? Id { get; set; }
        public string? Description { get; set; }
    }

    public class DeliveryAddress
    {
        public string? Region { get; set; }
    }

    public class Communication
    {
        public string? FutureNoticeDate { get; set; }
        public string? AtypicalToolUrl { get; set; }
    }

    public class SelectionCriteria
    {
        public List<Criteria>? Criteria { get; set; }
    }

    public class Document
    {
        public string? DocumentType { get; set; }
        public string? Id { get; set; }
    }

    public class OtherRequirements
    {
        public List<string>? ReservedParticipation { get; set; }
        public bool? RequiresStaffNamesAndQualifications { get; set; }
    }

    public class ContractTerms
    {
        public bool? ReservedExecution { get; set; }
        public string? PerformanceTerms { get; set; }
        public bool? HasElectronicOrdering { get; set; }
        public string? ElectronicInvoicingPolicy { get; set; }
        public bool? HasElectronicPayment { get; set; }
    }

    public class Techniques
    {
        public bool? HasFrameworkAgreement { get; set; }
        public FrameworkAgreement? FrameworkAgreement { get; set; }
        public bool? HasElectronicAuction { get; set; }
        public ElectronicAuction? ElectronicAuction { get; set; }
    }

    public class FrameworkAgreement
    {
        public string? MaximumParticipants { get; set; }
        public string? PeriodRationale { get; set; }
    }

    public class ElectronicAuction
    {
        public string? Description { get; set; }
    }

    public class AwardPeriod
    {
        public string? StartDate { get; set; }
    }

    public class Party
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public Identifier? Identifier { get; set; }
        public Address? Address { get; set; }
        public ContactPoint? ContactPoint { get; set; }
        public List<string>? Roles { get; set; }
        public Details? Details { get; set; }
    }

    public class Identifier
    {
        public string? LegalName { get; set; }
    }

    public class Address
    {
        public string? StreetAddress { get; set; }
        public string? Locality { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryName { get; set; }
    }

    public class ContactPoint
    {
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? FaxNumber { get; set; }
        public string? Url { get; set; }
    }

    public class Details
    {
        public string? Url { get; set; }
        public string? BuyerProfile { get; set; }
        public List<Classification>? Classifications { get; set; }
    }

    public class Buyer
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
