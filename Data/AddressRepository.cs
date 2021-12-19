using brotherscase.Domain;
using brotherscase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace brotherscase.Data
{
    public class AddressRepository
    {
        private AddressDbContext addressDbContext { get; }

        public AddressRepository(AddressDbContext addressDbContext)
        {
            this.addressDbContext = addressDbContext;
        }

        public IQueryable<Address> GetAddresses(AddressParameters addressParameters)
        {

            var addresses = FindByCondition(o => (addressParameters.Street == null || o.Street.Contains(addressParameters.Street)) &&
                                                 (addressParameters.HouseNumber == 0 || o.HouseNumber == addressParameters.HouseNumber) &&
                                                 (addressParameters.PostalCode == null || o.PostalCode.Contains(addressParameters.PostalCode)) &&
                                                 (addressParameters.Town == null || o.Town.Contains(addressParameters.Town)) &&
                                                 (addressParameters.Country == null || o.Country.Contains(addressParameters.Country)));

            ApplySort(ref addresses, addressParameters.OrderBy);

            return addresses;
        }

        public IQueryable<Address> FindByCondition(Expression<Func<Address, bool>> expression)
        {
            return addressDbContext.Set<Address>()
                .Where(expression)
                .AsNoTracking();
        }

        private void ApplySort(ref IQueryable<Address> addresses, string orderByQueryString) 
        { 
            if (!addresses.Any()) { return; }

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                addresses = addresses.OrderBy(x => x.Country);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Address).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            addresses =  addresses.OrderBy(orderQuery);
            return;
        }
    }
}
