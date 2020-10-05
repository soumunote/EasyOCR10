using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace EasyOCR10
{
    static class SoftwareBitmapExtention
    {
        public async static Task<SoftwareBitmap> Crop(this SoftwareBitmap bitmap, BitmapBounds bounds)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                encoder.SetSoftwareBitmap(bitmap);
                encoder.BitmapTransform.Bounds = bounds;

                await encoder.FlushAsync();

                var decoder = await BitmapDecoder.CreateAsync(stream);
                return await decoder.GetSoftwareBitmapAsync(bitmap.BitmapPixelFormat, BitmapAlphaMode.Premultiplied);
            }
        }

    }
}
