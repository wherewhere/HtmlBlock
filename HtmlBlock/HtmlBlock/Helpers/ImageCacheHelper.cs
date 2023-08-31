using HtmlBlock.Common;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace HtmlBlock.Helpers
{
    [Flags]
    public enum ImageType
    {
        Origin = 0x00,
        Small = 0x01,

        Image = 0x02,
        Avatar = 0x04,
        Icon = 0x08,
        Captcha = 0x16,

        OriginImage = Image | Origin,
        BigAvatar = Avatar | Origin,

        SmallImage = Image | Small,
        SmallAvatar = Avatar | Small,
    }

    public static partial class ImageCacheHelper
    {
        private static readonly Uri DarkNoPicUri = new Uri("ms-appx:/Assets/NoPic/img_placeholder_night.png");
        private static readonly Uri WhiteNoPicUri = new Uri("ms-appx:/Assets/NoPic/img_placeholder.png");

        private static Dictionary<CoreDispatcher, BitmapImage> DarkNoPicMode { get; set; } = new Dictionary<CoreDispatcher, BitmapImage>();
        private static Dictionary<CoreDispatcher, BitmapImage> WhiteNoPicMode { get; set; } = new Dictionary<CoreDispatcher, BitmapImage>();

        public static CoreDispatcher Dispatcher { get; } = CoreApplication.MainView.Dispatcher;

        static ImageCacheHelper() => ImageCache.Instance.CacheDuration = TimeSpan.FromHours(8);

        public static async Task<BitmapImage> GetImageAsync(ImageType type, string url, CoreDispatcher dispatcher, bool isForce = false)
        {
            if (!url.TryGetUri(out Uri uri)) { return await GetNoPicAsync(dispatcher); }

            if (url.IndexOf("ms-appx", StringComparison.Ordinal) == 0 || uri.IsFile)
            {
                if (!dispatcher.HasThreadAccess) { await dispatcher.ResumeForegroundAsync(); }
                return new BitmapImage(uri);
            }
            else if (!isForce && SettingsHelper.Get<bool>(SettingsHelper.IsNoPicsMode))
            {
                return await GetNoPicAsync(dispatcher);
            }
            else
            {
                if (type.HasFlag(ImageType.Small))
                {
                    if (url.Contains("coolapk.com") && !url.EndsWith(".png")) { url += ".s.jpg"; }
                    uri = url.TryGetUri();
                }

                if (await dispatcher.AwaitableRunAsync(() => Dispatcher.HasThreadAccess))
                {
                    try
                    {
                        BitmapImage image = await ImageCache.Instance.GetFromCacheAsync(uri, true);
                        return image;
                    }
                    catch (FileNotFoundException)
                    {
                        try
                        {
                            await ImageCache.Instance.RemoveAsync(new Uri[] { uri });
                            BitmapImage image = await ImageCache.Instance.GetFromCacheAsync(uri, true);
                            return image;
                        }
                        catch
                        {
                            return await GetNoPicAsync(dispatcher);
                        }
                    }
                    catch
                    {
                        return await GetNoPicAsync(dispatcher);
                    }
                }
                else
                {
                    StorageFile file = null;
                    try
                    {
                        file = await ImageCache.Instance.GetFileFromCacheAsync(uri);
                        if (file == null)
                        {
                            _ = await ImageCache.Instance.GetFromCacheAsync(uri, true);
                            file = await ImageCache.Instance.GetFileFromCacheAsync(uri);
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        try
                        {
                            await ImageCache.Instance.RemoveAsync(new Uri[] { uri });
                            _ = await ImageCache.Instance.GetFromCacheAsync(uri, true);
                            file = await ImageCache.Instance.GetFileFromCacheAsync(uri);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                    using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
                    {
                        await dispatcher.ResumeForegroundAsync();
                        BitmapImage image = new BitmapImage();
                        await image.SetSourceAsync(stream);
                        return image;
                    }
                }
            }
        }

        public static async Task<StorageFile> GetImageFileAsync(ImageType type, string url)
        {
            if (!url.TryGetUri(out Uri uri)) { return null; }

            if (url.IndexOf("ms-appx", StringComparison.Ordinal) == 0)
            {
                return await StorageFile.GetFileFromApplicationUriAsync(uri);
            }
            else if (uri.IsFile)
            {
                return await StorageFile.GetFileFromPathAsync(url);
            }
            else
            {
                if (type.HasFlag(ImageType.Small))
                {
                    if (url.Contains("coolapk.com") && !url.EndsWith(".png")) { url += ".s.jpg"; }
                    uri = url.TryGetUri();
                }

                try
                {
                    StorageFile image = await ImageCache.Instance.GetFileFromCacheAsync(uri);
                    if (image == null)
                    {
                        _ = await ImageCache.Instance.GetFromCacheAsync(uri, true);
                        image = await ImageCache.Instance.GetFileFromCacheAsync(uri);
                    }
                    return image;
                }
                catch (FileNotFoundException)
                {
                    try
                    {
                        await ImageCache.Instance.RemoveAsync(new Uri[] { uri });
                        _ = await ImageCache.Instance.GetFromCacheAsync(uri, true);
                        StorageFile image = await ImageCache.Instance.GetFileFromCacheAsync(uri);
                        return image;
                    }
                    catch
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public static Task CleanCacheAsync() => ImageCache.Instance.ClearAsync();

        public static BitmapImage GetNoPic(CoreDispatcher dispatcher)
        {
            if (!dispatcher.HasThreadAccess) { return null; }

            if (!DarkNoPicMode.ContainsKey(dispatcher))
            {
                DarkNoPicMode[dispatcher] = new BitmapImage(DarkNoPicUri) { DecodePixelHeight = 768, DecodePixelWidth = 768 };
                WhiteNoPicMode[dispatcher] = new BitmapImage(WhiteNoPicUri) { DecodePixelHeight = 768, DecodePixelWidth = 768 };
            }

            return ThemeHelper.IsDarkTheme()
                ? DarkNoPicMode[dispatcher]
                : WhiteNoPicMode[dispatcher];
        }

        public static async Task<BitmapImage> GetNoPicAsync(CoreDispatcher dispatcher)
        {
            if (!dispatcher.HasThreadAccess)
            {
                await dispatcher.ResumeForegroundAsync();
            }

            if (!DarkNoPicMode.ContainsKey(dispatcher))
            {
                DarkNoPicMode[dispatcher] = new BitmapImage(DarkNoPicUri) { DecodePixelHeight = 768, DecodePixelWidth = 768 };
                WhiteNoPicMode[dispatcher] = new BitmapImage(WhiteNoPicUri) { DecodePixelHeight = 768, DecodePixelWidth = 768 };
            }

            return ThemeHelper.IsDarkTheme()
                ? DarkNoPicMode[dispatcher]
                : WhiteNoPicMode[dispatcher];
        }
    }
}
