using GarageForFriends.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GarageForFriends
{
    public static class Utils
    {
        public static PromoContainer Promo { get; set; }
        public static MainPromoContainer MainPromo { get; set; }
        public static HeaderContainer Contact { get; set; }
        public static SliderContainer Slider { get; set; }
        public static YoutubeContainer Youtube { get; set; }
        public static NewsContainer News { get; set; }
        public static ServicesContainer Services { get; set; }
        public static CommentContainer Comments { get; set; }

        public static string EnviromentLocation = Path.Combine(Environment.CurrentDirectory, "Sync");

        public static byte[] downloadedFile { get; set; }
        public static Action<bool, dynamic> GetDownloadedFile = (isOk, obj) => {
            if (isOk) {
                downloadedFile = obj.ToArray();            
            }
        };


        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        public class ConcreteConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType) => true;

            public override object ReadJson(JsonReader reader,
             Type objectType, object existingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<T>(reader);
            }

            public override void WriteJson(JsonWriter writer,
                object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
