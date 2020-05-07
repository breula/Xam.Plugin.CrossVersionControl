using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xam.Plugin.CrossVersionControl
{
    /// <summary>
    /// Check versio plugin mais class
    /// </summary>
    public static class CheckVersion
    {
        #region-- Private --
        private static string OnlyNumbers(string valor)
        {
            return string.IsNullOrEmpty(valor) ? valor : string.Join("", Regex.Split(valor, @"[^\d]"));
        }
        #endregion

        #region-- Internal --
        internal class IosAppBundleData
        {
            [JsonProperty("resultCount")]
            public long ResultCount { get; set; }

            [JsonProperty("results")]
            public List<Result> Results { get; set; }
        }

        internal class Result
        {
            [JsonProperty("screenshotUrls")]
            public List<Uri> ScreenshotUrls { get; set; }

            [JsonProperty("ipadScreenshotUrls")]
            public List<object> IpadScreenshotUrls { get; set; }

            [JsonProperty("appletvScreenshotUrls")]
            public List<object> AppletvScreenshotUrls { get; set; }

            [JsonProperty("artworkUrl60")]
            public Uri ArtworkUrl60 { get; set; }

            [JsonProperty("artworkUrl512")]
            public Uri ArtworkUrl512 { get; set; }

            [JsonProperty("artworkUrl100")]
            public Uri ArtworkUrl100 { get; set; }

            [JsonProperty("artistViewUrl")]
            public Uri ArtistViewUrl { get; set; }

            [JsonProperty("supportedDevices")]
            public List<string> SupportedDevices { get; set; }

            [JsonProperty("advisories")]
            public List<object> Advisories { get; set; }

            [JsonProperty("isGameCenterEnabled")]
            public bool IsGameCenterEnabled { get; set; }

            [JsonProperty("kind")]
            public string Kind { get; set; }

            [JsonProperty("features")]
            public List<object> Features { get; set; }

            [JsonProperty("trackCensoredName")]
            public string TrackCensoredName { get; set; }

            [JsonProperty("languageCodesISO2A")]
            public List<object> LanguageCodesIso2A { get; set; }

            [JsonProperty("fileSizeBytes")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long FileSizeBytes { get; set; }

            [JsonProperty("contentAdvisoryRating")]
            public string ContentAdvisoryRating { get; set; }

            [JsonProperty("averageUserRatingForCurrentVersion")]
            public long AverageUserRatingForCurrentVersion { get; set; }

            [JsonProperty("userRatingCountForCurrentVersion")]
            public long UserRatingCountForCurrentVersion { get; set; }

            [JsonProperty("averageUserRating")]
            public long AverageUserRating { get; set; }

            [JsonProperty("trackViewUrl")]
            public Uri TrackViewUrl { get; set; }

            [JsonProperty("trackContentRating")]
            public string TrackContentRating { get; set; }

            [JsonProperty("trackId")]
            public long TrackId { get; set; }

            [JsonProperty("trackName")]
            public string TrackName { get; set; }

            [JsonProperty("releaseDate")]
            public DateTimeOffset ReleaseDate { get; set; }

            [JsonProperty("genreIds")]
            [JsonConverter(typeof(DecodeArrayConverter))]
            public List<long> GenreIds { get; set; }

            [JsonProperty("formattedPrice")]
            public string FormattedPrice { get; set; }

            [JsonProperty("primaryGenreName")]
            public string PrimaryGenreName { get; set; }

            [JsonProperty("isVppDeviceBasedLicensingEnabled")]
            public bool IsVppDeviceBasedLicensingEnabled { get; set; }

            [JsonProperty("minimumOsVersion")]
            public string MinimumOsVersion { get; set; }

            [JsonProperty("sellerName")]
            public string SellerName { get; set; }

            [JsonProperty("currentVersionReleaseDate")]
            public DateTimeOffset CurrentVersionReleaseDate { get; set; }

            [JsonProperty("releaseNotes")]
            public string ReleaseNotes { get; set; }

            [JsonProperty("primaryGenreId")]
            public long PrimaryGenreId { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("wrapperType")]
            public string WrapperType { get; set; }

            [JsonProperty("artistId")]
            public long ArtistId { get; set; }

            [JsonProperty("artistName")]
            public string ArtistName { get; set; }

            [JsonProperty("genres")]
            public List<string> Genres { get; set; }

            [JsonProperty("price")]
            public long Price { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("bundleId")]
            public string BundleId { get; set; }

            [JsonProperty("userRatingCount")]
            public long UserRatingCount { get; set; }
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                long l;
                if (long.TryParse(value, out l))
                {
                    return l;
                }
                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }

        internal class DecodeArrayConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(List<long>);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                reader.Read();
                var value = new List<long>();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    var converter = ParseStringConverter.Singleton;
                    var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                    value.Add(arrayItem);
                    reader.Read();
                }
                return value;
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                var value = (List<long>)untypedValue;
                writer.WriteStartArray();
                foreach (var arrayItem in value)
                {
                    var converter = ParseStringConverter.Singleton;
                    converter.WriteJson(writer, arrayItem, serializer);
                }
                writer.WriteEndArray();
                return;
            }

            public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
        }

        #endregion

        /// <summary>
        /// Make store current version verification
        /// </summary>
        /// <param name="appBandleId">Bandle Name for app package</param>
        /// <returns>VerifyVersionResult</returns>
        public static VerifyVersionResult VerifyAndroid(string appBandleId)
        {
            var verification = new VerifyVersionResult();

            string url = $"https://play.google.com/store/apps/details?id={appBandleId}&hl=en";

            using (var webClient = new WebClient())
            {
                var donwloadedString = webClient.DownloadString(url);
                var jsonString = donwloadedString;

                #region-- Get Name --
                var searchString = "itemprop=\"name\">";
                var endString = "</";
                var pos = jsonString.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) + searchString.Length;
                var endPos = jsonString.IndexOf(endString, pos, StringComparison.Ordinal);
                jsonString = jsonString.Substring(pos, endPos - pos).Trim();
                verification.AppName = jsonString.Substring(jsonString.IndexOf(">") + 1, jsonString.Length - jsonString.LastIndexOf(">") - 1).Trim();
                #endregion

                #region-- Get Version --
                jsonString = donwloadedString;
                searchString = "Current Version";
                endString = "</span";
                pos = jsonString.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) + searchString.Length;
                endPos = jsonString.IndexOf(endString, pos, StringComparison.Ordinal);
                jsonString = jsonString.Substring(pos, endPos - pos).Trim();
                verification.StoreCurrentVersion = jsonString.Substring(jsonString.LastIndexOf(">") + 1, jsonString.Length - jsonString.LastIndexOf(">") - 1);
                #endregion

                verification.VersioningType = string.IsNullOrEmpty(OnlyNumbers(verification.StoreCurrentVersion))
                    ? VersioningType.ByCodeName
                    : VersioningType.ByIncrementalNumber;

                return verification;
            }
        }

        /// <summary>
        /// Make store current version verification
        /// </summary>
        /// <param name="appBandleId">Bandle Name for app package</param>
        /// <returns>VerifyVersionResult</returns>
        public static VerifyVersionResult VerifyIos(string appBandleId)
        {
            var verification = new VerifyVersionResult();

            string url = $"http://itunes.apple.com/lookup?bundleId={appBandleId}";

            using (var webClient = new WebClient())
            {
                var donwloadedString = webClient.DownloadString(url);

                var data = JsonConvert.DeserializeObject<IosAppBundleData>(donwloadedString);

                if (data.ResultCount == 0)
                {

                }

                verification.AppName = data.Results[0].TrackName;
                verification.StoreCurrentVersion = data.Results[0].Version;
                verification.VersioningType = string.IsNullOrEmpty(OnlyNumbers(verification.StoreCurrentVersion))
                    ? VersioningType.ByCodeName
                    : VersioningType.ByIncrementalNumber;
            }

            return verification;
        }
    }
}
