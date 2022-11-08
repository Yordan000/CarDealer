using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    [JsonObject]

    public class ImportCarsPartsDto
    {
        public int PartsId { get; set; }
    }
}
