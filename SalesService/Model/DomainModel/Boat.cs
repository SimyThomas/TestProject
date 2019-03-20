using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalesService.Model.DomainModel
{
    public class Boat
    {
        /// <summary>Gets or sets the Model Year.</summary>
        public string ModelYear { get; set; }

        /// <summary>Gets or sets the BuilderName</summary>
        public string BuilderName { get; set; }

        /// <summary>Gets or sets the Model Name </summary>
        public string Model { get; set; }

        /// <summary>Gets or sets the type of Watercraft.</summary>
        public string WatercraftType { get; set; }

        /// <summary>Gets or sets the EngineType</summary>
        public string EngineType { get; set; }

        /// <summary>Gets or sets whether it is customized.</summary>
        /// <value>The is customized</value>
        public bool? IsCustomized { get; set; }

        /// <summary>Gets or sets the Boat identifier</summary>
        public int Identifier { get; set; }


    }
}
