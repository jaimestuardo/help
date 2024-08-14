using Microsoft.Extensions.Configuration;

#if ANDROID
using TimeApp.Platforms.Android;
#elif IOS
using TimeApp.Platforms.iOS;
#endif

namespace TimeApp.Services
{
    public class PhotoService
    {
        private readonly int maxWidth = 0;
        private readonly int maxHeight = 0;

        public PhotoService(IConfiguration config)
        {
            var settings = config.GetRequiredSection("Settings").Get<Settings>();
            maxWidth = settings.Photo.MaxWidth;
            maxHeight = settings.Photo.MaxHeight;
        }

        public async Task<PictureItem> TakePhotoAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    string extension = Path.GetExtension(photo.FileName);
                    Stream stream = await photo.OpenReadAsync();

                    byte[] imageData;
                    using (MemoryStream ms = new())
                    {
                        stream.CopyTo(ms);
                        imageData = ms.ToArray();
                    }
                    if (imageData.Length > 524288 && maxWidth != 0 && maxHeight != 0) // Greater then 512 kb then do resize
                    {
#if ANDROID
                        imageData = RImageHelper.ReDimAndRotate(imageData, maxWidth, maxHeight);
#elif IOS
                        imageData = RImageHelper.ReDimAndRotate(imageData, maxWidth, maxHeight);
#endif
                    }

                    return new PictureItem
                    {
                        Extension = extension.Length == 0 ? string.Empty : extension[1..],
                        Stream = new MemoryStream(imageData)
                    };
                }
            }

            return null;
        }
    }
}
