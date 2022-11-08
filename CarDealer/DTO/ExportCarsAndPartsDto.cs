using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    [JsonObject]
    public class ExportCarsAndPartsDto
    {
        [JsonProperty("Make")]
        public string Make { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("TravelledDistance")]
        public long TravelledDistance { get; set; }

        [JsonProperty("parts")]
        public ExportPartsFromCarsDto[] Parts { get; set; }
    }
}
