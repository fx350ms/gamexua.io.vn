namespace GameXuaVN.Web.Models.File
{
    public class FileViewModel
    {
        public string FileName { get; set; }        // Tên của file
        public string Description { get; set; }     // Mô tả về file
        public int DownloadCount { get; set; }      // Số lượt tải xuống
        public float AverageRating { get; set; }    // Điểm đánh giá trung bình
        public string ThumbnailPath { get; set; }   // Đường dẫn tới ảnh thumbnail của file (nếu có)
    }
}
