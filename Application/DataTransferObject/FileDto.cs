namespace QAP4.Application.DataTransferObjects
{
    public class FileDto
    {
        public bool Success { get; set; }
        public string Url { get; set; }
        public string PreSignedURL { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}