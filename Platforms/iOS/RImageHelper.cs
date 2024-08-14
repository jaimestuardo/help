using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace TimeApp.Platforms.iOS
{
    public static class RImageHelper
    {
        public static byte[] ReDimAndRotate(byte[] imageData, int width, int height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using CGBitmapContext context = new(IntPtr.Zero,
                                                 width, height, 8,
                                                 4 * width, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst);

            RectangleF imageRect = new(0, 0, width, height);

            // draw the image
            context.DrawImage(imageRect, originalImage.CGImage);
            //rotated 90° counterclockwise from the orientation of its original pixel data.
            UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, UIImageOrientation.Left);

            // save the image as a jpeg
            return [.. resizedImage.AsJPEG()];
        }

        private static UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}
