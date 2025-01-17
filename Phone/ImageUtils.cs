using Android.Content;
using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Graphics.ImageDecoder;

namespace Phone
{
    public static class ImageUtils
    {
        /// <summary>
        /// Создает закругленное изображение из исходного Bitmap.
        /// </summary>
        /// <param name="source">Исходное изображение.</param>
        /// <returns>Закругленное изображение.</returns>
        public static Bitmap GetRoundedBitmap(Bitmap source)
        {
            int width = source.Width;
            int height = source.Height;
            float radius = Math.Min(width, height) / 2f;

            Bitmap output = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);

            Paint paint = new Paint
            {
                AntiAlias = true
            };
            Rect rect = new Rect(0, 0, width, height);

            float cx = width / 2f;
            float cy = height / 2f;

            // Рисуем круг
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = Android.Graphics.Color.White;
            canvas.DrawCircle(cx, cy, radius, paint);

            // Комбинируем изображение с кругом
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(source, rect, rect, paint);

            return output;
        }

        /// <summary>
        /// Создает закругленное изображение из ресурса.
        /// </summary>
        /// <param name="context">Контекст приложения.</param>
        /// <param name="drawableId">Идентификатор ресурса изображения.</param>
        /// <returns>Закругленное изображение.</returns>
        public static Bitmap GetRoundedBitmap(Context context, int drawableId)
        {
            Bitmap bitmap = BitmapFactory.DecodeResource(context.Resources, drawableId);
            return GetRoundedBitmap(bitmap);
        }
    }
}
