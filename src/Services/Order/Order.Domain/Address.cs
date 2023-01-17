using Order.Domain.Shared;

namespace Order.Domain
{
    public class Address : ValueObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return EmailAddress;
            yield return AddressLine;
            yield return Country;
            yield return State;
            yield return ZipCode;
        }
    }
}