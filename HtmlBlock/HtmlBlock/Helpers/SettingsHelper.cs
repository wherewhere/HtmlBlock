using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.Toolkit.Helpers;
using IObjectSerializer = Microsoft.Toolkit.Helpers.IObjectSerializer;
using Microsoft.Toolkit.Uwp.Helpers;

namespace HtmlBlock.Helpers
{
    public static partial class SettingsHelper
    {
        public const string IsNoPicsMode = nameof(IsNoPicsMode);
        public const string IsUseOldEmojiMode = nameof(IsUseOldEmojiMode);
        public const string IsDisplayOriginPicture = nameof(IsDisplayOriginPicture);

        public static Type Get<Type>(string key) => LocalObject.Read<Type>(key);
        public static void Set<Type>(string key, Type value) => LocalObject.Save(key, value);
        public static Task<Type> GetFile<Type>(string key) => LocalObject.ReadFileAsync<Type>($"Settings/{key}");
        public static Task SetFile<Type>(string key, Type value) => LocalObject.CreateFileAsync($"Settings/{key}", value);

        public static void SetDefaultSettings()
        {
            if (!LocalObject.KeyExists(IsNoPicsMode))
            {
                LocalObject.Save(IsNoPicsMode, false);
            }
            if (!LocalObject.KeyExists(IsUseOldEmojiMode))
            {
                LocalObject.Save(IsUseOldEmojiMode, false);
            }
            if (!LocalObject.KeyExists(IsDisplayOriginPicture))
            {
                LocalObject.Save(IsDisplayOriginPicture, false);
            }
        }
    }

    public static partial class SettingsHelper
    {
        public static readonly ApplicationDataStorageHelper LocalObject = ApplicationDataStorageHelper.GetCurrent(new SystemTextJsonObjectSerializer());

        static SettingsHelper() => SetDefaultSettings();
    }

    public class SystemTextJsonObjectSerializer : IObjectSerializer
    {
        // Specify your serialization settings
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore };

        string IObjectSerializer.Serialize<T>(T value) => JsonConvert.SerializeObject(value, typeof(T), Formatting.Indented, settings);

        public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value, settings);
    }
}
