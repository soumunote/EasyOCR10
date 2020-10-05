using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyOCR10
{
    public sealed partial class ScanningDialog : ContentDialog
    {
        public ScanningDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            await doScanAsync();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        async Task doScanAsync()
        {
            var ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            if (ocrEngine == null)
            {
                //TODO
                //rootPage.NotifyUser();
            }
            // https://github.com/microsoft/Windows-universal-samples/blob/master/Samples/OCR/cs/OcrCapturedImage.xaml.cs
            // https://docs.microsoft.com/ja-jp/windows/uwp/files/quickstart-reading-and-writing-files
            // https://docs.microsoft.com/ja-jp/windows/uwp/audio-video-camera/imaging
            // https://codezine.jp/article/detail/11072?p=2
            // https://www.tnksoft.com/blog/?p=5634
            var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(@"c:\tmp\ticket");
            var imageFile = await storageFolder.GetFileAsync("1.jpg");
            using (var stream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var bitmap = await decoder.GetSoftwareBitmapAsync();
                var ocrResult = await ocrEngine.RecognizeAsync(bitmap);
                var text = ocrResult.Text;

            }

        }


    }
}
