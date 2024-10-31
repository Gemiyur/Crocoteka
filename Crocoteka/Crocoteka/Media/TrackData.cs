using System.Windows.Media.Imaging;
using ATL;

namespace Crocoteka.Media
{
    public class TrackData
    {
        public string Title;

        public string Description;

        public string Comment;

        public string Album;

        public string AlbumArtist;

        public string Artist;

        public List<ChapterData> Chapters = [];

        public TimeSpan Duration;

        public string Genre;

        public string Lyric;

        public List<MarkData> Marks = [];

        public List<BitmapFrame> Pictures = [];

        public List<byte[]> PicturesData = [];

        public string SeriesTitle;

        public string SeriesPart;

        public int? Year;

        public TrackData(string filename)
        {
            var track = new Track(filename);
            Title = track.Title;
            Description = track.Description;
            Comment = track.Comment;
            Album = track.Album;
            AlbumArtist = track.AlbumArtist;
            Artist = track.Artist;
            foreach (var chapter in track.Chapters)
            {
                var chapterData = new ChapterData()
                {
                    Title = chapter.Title,
                    StartTime = TimeSpan.FromMilliseconds(chapter.StartTime),
                    EndTime = TimeSpan.FromMilliseconds(chapter.EndTime),
                    Picture = chapter.Picture != null ? App.GetBitmap(chapter.Picture.PictureData) : null,
                    PictureData = chapter.Picture?.PictureData
                };
                Chapters.Add(chapterData);
            }
            Duration = TimeSpan.FromSeconds(track.Duration);
            Genre = track.Genre;
            Lyric = track.Lyrics.UnsynchronizedLyrics;
            foreach (var mark in track.Lyrics.SynchronizedLyrics)
            {
                Marks.Add(new MarkData() { Title = mark.Text, Position = TimeSpan.FromMilliseconds(mark.TimestampMs) });
            }
            foreach (var picture in track.EmbeddedPictures)
            {
                Pictures.Add(App.GetBitmap(picture.PictureData));
                PicturesData.Add(picture.PictureData);
            }
            SeriesTitle = track.SeriesTitle;
            SeriesPart = track.SeriesPart;
            Year = track.Year;
        }
    }
}
