using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Filters
{
    public class AddressFilter
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public Expression<Func<Record, bool>> BuildFilterExpression()
        {
            Expression<Func<Record, bool>> filter = r => true;

            if (!string.IsNullOrWhiteSpace(Street))
                filter = filter.And(r => r.Address.Street.Contains(Street));

            if (!string.IsNullOrWhiteSpace(City))
                filter = filter.And(r => r.Address.City.Contains(City));

            if (!string.IsNullOrWhiteSpace(State))
                filter = filter.And(r => r.Address.State.Contains(State));

            if (!string.IsNullOrWhiteSpace(PostalCode))
                filter = filter.And(r => r.Address.PostalCode == PostalCode);

            if (!string.IsNullOrWhiteSpace(Country))
                filter = filter.And(r => r.Address.Country.Contains(Country));
            return filter;
        }
    }
}
