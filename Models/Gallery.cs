namespace GiveAID.Models
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public string Category { get; set; }  
        public DateTime UploadedAt { get; set; }   
    }
}
