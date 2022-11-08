using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    [JsonObject]
    public class ExportCarsCarInfoDto
    {
        [JsonProperty("car")]
        public ExportCarsAndPartsDto Car { get; set; }

        [JsonProperty("parts")]
        public ExportPartsFromCarsDto[] Parts { get; set; }
    }
}
