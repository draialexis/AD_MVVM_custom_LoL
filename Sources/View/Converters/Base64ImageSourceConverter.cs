using System.Globalization;

namespace View.Converters
{
    public class Base64ImageSourceConverter : IValueConverter
    {
        private const string DefaultBase64Image = "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";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string base64Image || string.IsNullOrWhiteSpace(base64Image))
            {
                base64Image = DefaultBase64Image;
            }

            byte[] imageBytes = System.Convert.FromBase64String(base64Image);

            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
