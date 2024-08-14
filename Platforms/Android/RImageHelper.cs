using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Platforms.Android
{
    public static class RImageHelper
    {
        public static byte[] ReDimAndRotate(byte[] imageData, int width, int height)
        {
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Matrix matrix = new();
            matrix.SetRotate(270);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, width, height, false);
            Bitmap RotatedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, matrix, true);
            using MemoryStream ms = new();
            RotatedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
            return ms.ToArray();
        }
    }
}
