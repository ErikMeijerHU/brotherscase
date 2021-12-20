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
            OrderBy = "addressId";
        }
        /// <summary>Example: Heidelberglaan</summary>
        public string Street { get; set; }
        /// <summary>Example: 15</summary>
        public int HouseNumber { get; set; }
        /// <summary>Example: 3584CS</summary>
        public string PostalCode { get; set; }
        /// <summary>Example: Utrecht</summary>
        public string Town { get; set; }
        /// <summary>Example: Nederland</summary>
        public string Country { get; set; }
        /// <summary>Example: housenumber desc,street</summary>
        public string OrderBy { get; set; }
    }
}
