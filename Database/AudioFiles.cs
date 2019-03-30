using System.Xml;

namespace Database
{
    public class AudioFile
    {
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
        
        public string FullPath { get; set; }
    }
}