using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace MyCupOfCoffeeService.DataObjects
{
    public class CupOfCoffee : EntityData
    {
        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the date UTC.
        /// </summary>
        /// <value>The date UTC.</value>
        public DateTime DateUtc { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="CupOfCoffee" /> made at home.
        /// </summary>
        /// <value><c>true</c> if made at home; otherwise, <c>false</c>.</value>
        public bool MadeAtHome { get; set; }

        /// <summary>
        ///     Gets or sets the location of the coffee
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///     Gets or sets the OS of the user
        /// </summary>
        /// <value>The OS</value>
        public string OS { get; set; }

    }
}