using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Media
{
    public class SelfieMediaPickerImplementation : IMediaPicker
    {
        /*
        public bool IsCaptureSupported => Application.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraAny);

        public Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null) => CaptureAsync(options, true);

        public Task<FileResult> CaptureVideoAsync(MediaPickerOptions options = null) => CaptureAsync(options, false);

        public Task<FileResult> PickPhotoAsync(MediaPickerOptions options = null) => PickAsync(options, true);

        public Task<FileResult> PickVideoAsync(MediaPickerOptions options = null) => PickAsync(options, false);

        public async Task<FileResult> PickAsync(MediaPickerOptions options, bool photo)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType(photo ? FileMimeTypes.ImageAll : FileMimeTypes.VideoAll);

            var pickerIntent = Intent.CreateChooser(intent, options?.Title);

            try
            {
                string path = null;
                void OnResult(Intent intent)
                {
                    // The uri returned is only temporary and only lives as long as the Activity that requested it,
                    // so this means that it will always be cleaned up by the time we need it because we are using
                    // an intermediate activity.

                    path = FileSystemUtils.EnsurePhysicalPath(intent.Data);
                }

                await IntermediateActivity.StartAsync(pickerIntent, PlatformUtils.requestCodeMediaPicker, onResult: OnResult);

                return new FileResult(path);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public async Task<FileResult> CaptureAsync(MediaPickerOptions options, bool photo)
        {
            if (!IsCaptureSupported)
                throw new FeatureNotSupportedException();

            await Permissions.EnsureGrantedAsync<Permissions.Camera>();
            // StorageWrite no longer exists starting from Android API 33
            if (!OperatingSystem.IsAndroidVersionAtLeast(33))
                await Permissions.EnsureGrantedAsync<Permissions.StorageWrite>();

            var captureIntent = new Intent(photo ? MediaStore.ActionImageCapture : MediaStore.ActionVideoCapture);

            if (!PlatformUtils.IsIntentSupported(captureIntent))
                throw new FeatureNotSupportedException($"Either there was no camera on the device or '{captureIntent.Action}' was not added to the <queries> element in the app's manifest file. See more: https://developer.android.com/about/versions/11/privacy/package-visibility");

            captureIntent.PutExtra("android.intent.extras.CAMERA_FACING", 1);

            captureIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            captureIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);

            try
            {
                var activity = ActivityStateManager.Default.GetCurrentActivity(true);

                string captureResult = null;

                if (photo)
                    captureResult = await CapturePhotoAsync(captureIntent);
                else
                    captureResult = await CaptureVideoAsync(captureIntent);

                // Return the file that we just captured
                return new FileResult(captureResult);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        async Task<string> CapturePhotoAsync(Intent captureIntent)
        {
            // Create the temporary file
            var fileName = Guid.NewGuid().ToString("N") + FileExtensions.Jpg;
            var tmpFile = FileSystemUtils.GetTemporaryFile(Application.Context.CacheDir, fileName);

            // Set up the content:// uri
            AndroidUri outputUri = null;

            void OnCreate(Intent intent)
            {
                // Android requires that using a file provider to get a content:// uri for a file to be called
                // from within the context of the actual activity which may share that uri with another intent
                // it launches.
                outputUri ??= FileProvider.GetUriForFile(tmpFile);

                intent.PutExtra(MediaStore.ExtraOutput, outputUri);
            }

            await IntermediateActivity.StartAsync(captureIntent, PlatformUtils.requestCodeMediaCapture, OnCreate);

            return tmpFile.AbsolutePath;
        }

        async Task<string> CaptureVideoAsync(Intent captureIntent)
        {
            string path = null;

            void OnResult(Intent intent)
            {
                // The uri returned is only temporary and only lives as long as the Activity that requested it,
                // so this means that it will always be cleaned up by the time we need it because we are using
                // an intermediate activity.
                path = FileSystemUtils.EnsurePhysicalPath(intent.Data);
            }

            // Start the capture process
            await IntermediateActivity.StartAsync(captureIntent, PlatformUtils.requestCodeMediaCapture, onResult: OnResult);

            return path;
        }
        */
        
        public bool IsCaptureSupported => throw new NotImplementedException();

        public Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<FileResult> CaptureVideoAsync(MediaPickerOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<FileResult> PickPhotoAsync(MediaPickerOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<FileResult> PickVideoAsync(MediaPickerOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
