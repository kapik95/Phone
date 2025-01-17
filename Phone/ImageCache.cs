using Android.Graphics;
using System.Collections.Concurrent;

namespace Phone
{
    public static class ImageCache
    {
        private static readonly ConcurrentDictionary<string, Bitmap> Cache = new ConcurrentDictionary<string, Bitmap>();

        public static void AddBitmap(string key, Bitmap bitmap)
        {
            Cache.TryAdd(key, bitmap);
        }

        public static Bitmap GetBitmap(string key) 
        {
            Cache.TryGetValue(key, out Bitmap bitmap);
            return bitmap;
        }
    }
}
