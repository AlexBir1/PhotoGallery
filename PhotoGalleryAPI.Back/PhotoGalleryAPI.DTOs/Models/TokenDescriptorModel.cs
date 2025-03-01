namespace PhotoGalleryAPI.Shared
{
    public class TokenDescriptorModel
    {
        public string Key { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
