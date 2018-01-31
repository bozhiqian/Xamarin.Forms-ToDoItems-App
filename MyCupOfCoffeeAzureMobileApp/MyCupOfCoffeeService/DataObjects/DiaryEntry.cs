using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace MyCupOfCoffeeService.DataObjects
{
    /// <summary>
    /// A single diary entry in our Azure table.
    /// </summary>
    [JsonObject(Title = "diaryentry3")]
    public class DiaryEntry : EntityData
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Diary entry text
        /// </summary>
        public string Text { get; set; }

        // Lab4: added UserId field
        /// <summary>
        /// UserId that created this record
        /// </summary>
        public string UserId { get; set; }

    }
}