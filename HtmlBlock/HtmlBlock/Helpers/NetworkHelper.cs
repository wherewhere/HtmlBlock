using System;

namespace HtmlBlock.Helpers
{
    public static partial class NetworkHelper
    {
        public static Uri TryGetUri(this string url)
        {
            url.TryGetUri(out Uri uri);
            return uri;
        }

        public static bool TryGetUri(this string url, out Uri uri)
        {
            uri = default;
            if (string.IsNullOrWhiteSpace(url)) { return false; }
            try
            {
                return url.Contains(":")
                    ? Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri)
                    : Uri.TryCreate($"https://{url}", UriKind.RelativeOrAbsolute, out uri);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
