namespace Models
{
    public class MinioOptions
    {
        public const string SectionName = "Storage:MinIO";
        public string? Endpoint { get; set; }
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
        public string BucketName { get; set; } = "pawshare-uploads";
        public bool UseSSL { get; set; }
        public bool IsConfigured => !string.IsNullOrWhiteSpace(Endpoint) && !string.IsNullOrWhiteSpace(AccessKey) && !string.IsNullOrWhiteSpace(SecretKey);
    }
}