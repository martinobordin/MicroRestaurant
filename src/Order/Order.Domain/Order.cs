using Order.Domain.Shared;

namespace Order.Domain
{
    public class Order : Entity
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        /// <value>
        /// The total price.
        /// </value>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        /// <value>
        /// The billing address.
        /// </value>
        public Address? BillingAddress { get; set; }
    }
}