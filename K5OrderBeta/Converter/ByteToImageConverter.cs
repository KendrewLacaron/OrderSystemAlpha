using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace K5OrderBeta.Converter
{
    public class ByteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource imageSource = null;
            if (value != null)
            {
                byte[] image = (byte[])value;
                imageSource = ImageSource.FromStream(() => new MemoryStream(image));
            }
            return imageSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
