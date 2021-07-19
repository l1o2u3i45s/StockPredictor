﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using EODHistoricalData.NET;
//
//    var indexComposition = IndexComposition.FromJson(jsonString);

namespace EODHistoricalData.NET
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using EODHistoricalData.NET.BusinessObjects;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class IndexComposition
    {
        [JsonProperty("General")]
        public FundamentalGeneral General { get; set; }

        [JsonProperty("Components")]
        public Dictionary<string, Component> Components { get; set; }
    }

    public partial class Component
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Exchange")]
        public string Exchange { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Sector")]
        public string Sector { get; set; }

        [JsonProperty("Industry")]
        public string Industry { get; set; }
    }
    
    public partial class IndexComposition
    {
        public static IndexComposition FromJson(string json) => JsonConvert.DeserializeObject<IndexComposition>(json, EODHistoricalData.NET.ConverterIndexComposition.Settings);
    }

    public static class SerializeIndexComposition
    {
        public static string ToJson(this IndexComposition self) => JsonConvert.SerializeObject(self, EODHistoricalData.NET.ConverterIndexComposition.Settings);
    }

    internal static class ConverterIndexComposition
    {
        public static List<string> Errors = new List<string>();
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                new NullConverter(),
            },
            Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
            {
                Errors.Add(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            },
        };
    }
}