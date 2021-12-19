using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brotherscase.Models
{
    public class AddressParameters
    {
        public AddressParameters() 
        {
            OrderBy = "country,town,street";
        }

        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }

        public string OrderBy { get; set; }
    }
}
