using System.Windows.Media.Imaging;

namespace Crocoteka.Media
{
    public class ChapterData
    {
        public string Title = string.Empty;

        public TimeSpan StartTime;

        public TimeSpan EndTime;

        public BitmapFrame? Picture;

        public byte[]? PictureData;
    }
}
