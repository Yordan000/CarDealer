using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    [JsonObject]
    public class ExportOrderedCustomersDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("BirthDate")]
        public string BirthDate { get; set; }

        [JsonProperty("IsYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
