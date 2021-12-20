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
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Geolocation;

namespace brotherscase.Data
{
    public class AddressRepository
    {
        private AddressDbContext addressDbContext { get; }

        public AddressRepository(AddressDbContext addressDbContext)
        {
            this.addressDbContext = addressDbContext;
        }

        public double GetDistanceInKilometers(Address firstAddress, Address secondAddress) 
        {
            var fCoords = GetCoordsByAddress(firstAddress);
            var sCoords = GetCoordsByAddress(secondAddress);

            return (GeoCalculator.GetDistance(fCoords, sCoords, 1)) * 1.609344;
        }

        private Coordinate GetCoordsByAddress(Address address)
        {
            //Api key moet netter opgeslagen, maakt voor nu niet zo uit.
            string requestUri = string.Format("http://api.positionstack.com/v1/forward?access_key={1}&query={0}", Uri.EscapeDataString(address.ToString()), "7ecefe487ef588484e946011be8652b2");

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse webResponse = request.GetResponse();

            String response = "";

            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                response = reader.ReadToEnd();
            }

            JObject joResponse = JObject.Parse(response);
            JArray array = (JArray)joResponse["data"];
            JObject ojObject = (JObject)array[0];
            double lat = (double)ojObject["latitude"];
            double lon = (double)ojObject["longitude"];

            var coords = new Coordinate(lat, lon);
            return coords;
        }

        public IQueryable<Address> GetAddresses(AddressParameters addressParameters)
        {

            // Met meer tijd zou ik hier een soort expression builder van maken die alleen de checks toevoegd aan de expression waarbij de parameter niet leeg is door middel van looping
            Expression<Func<Address, bool>> expression = o => (addressParameters.Street == null || o.Street.ToLower().Contains(addressParameters.Street.ToLower())) &&
                                                              (addressParameters.HouseNumber == 0 || o.HouseNumber == addressParameters.HouseNumber) &&
                                                              (addressParameters.PostalCode == null || o.PostalCode.ToLower().Contains(addressParameters.PostalCode.ToLower())) &&
                                                              (addressParameters.Town == null || o.Town.ToLower().Contains(addressParameters.Town.ToLower())) &&
                                                              (addressParameters.Country == null || o.Country.ToLower().Contains(addressParameters.Country.ToLower()));

            var addresses = FindByCondition(expression);

            ApplySort(ref addresses, addressParameters.OrderBy);

            return addresses;
        }

        
        private IQueryable<Address> FindByCondition(Expression<Func<Address, bool>> expression)
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
