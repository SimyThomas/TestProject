using Newtonsoft.Json;
using SalesService.Model.DomainModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalesService.Model
{
    public class Boats
    {
        /// <summary>public property of boats</summary>
        [JsonProperty(PropertyName = "Boats")]
        public ICollection<Boat> BoatCollection { get; set; }

        /// <summary>constructor of Boats collection</summary>
        public Boats()
        {
            this.BoatCollection = (ICollection<Boat>)new Collection<Boat>();
        }
    }
}
