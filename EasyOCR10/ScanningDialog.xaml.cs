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
using Windows.UI.Popups;
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
            // https://codezine.jp/article/detail/10748?p=3
            // https://qiita.com/kob58im/items/cdc0dbb93cf7f00e5144
            // https://rpa.bigtreetc.com/column/microsoftocr/
            // https://ishigame-machine-technology.net/IMT/archives/1306
            // https://stackoverflow.com/questions/46544423/how-to-crop-softwarebitmap-in-uwp-c-sharp
            // https://docs.microsoft.com/ja-jp/windows/uwp/design/controls-and-patterns/dialogs-and-flyouts/dialogs
            // https://github.com/microsoft/Windows-universal-samples/blob/master/Samples/OCR/cs/OcrCapturedImage.xaml.cs
            // https://docs.microsoft.com/ja-jp/windows/uwp/files/quickstart-reading-and-writing-files
            // https://docs.microsoft.com/ja-jp/windows/uwp/audio-video-camera/imaging
            // https://codezine.jp/article/detail/11072?p=2
            // https://www.tnksoft.com/blog/?p=5634
            var imageExtentions = new[] { "jpg", "jpeg", "png", "bmp" };

            var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(@"c:\tmp\ticket");
            var files = await folder.GetFilesAsync();
            var imageFiles = files.Where(file => imageExtentions.Contains(file.FileType)).ToArray();
            foreach (var imageFile in files.ToArray())
            {
                using (var stream = await imageFile.OpenAsync(FileAccessMode.Read))
                {
                    var decoder = await BitmapDecoder.CreateAsync(stream);
                    var bitmap = await decoder.GetSoftwareBitmapAsync();

                    var boundsAry = new[]
                    {
                        new BitmapBounds
                        {
                            X      =        1500, Y      =        75, 
                            Width  = 1875 - 1500, Height =  170 -  75
                        },
                         new BitmapBounds
                        {
                            X     =         720, Y      =        690,
                            Width = 1230 -  720, Height =  800 - 690
                        },
                    };

                    foreach (var bounds in boundsAry)
                    {
                        var fieldBitmap = await bitmap.Crop(bounds);
                        var ocrResult = await ocrEngine.RecognizeAsync(fieldBitmap);
                        var text = ocrResult.Text;
                        await (new MessageDialog(text)).ShowAsync();
                    };

                }

            }
        }


    }
}
