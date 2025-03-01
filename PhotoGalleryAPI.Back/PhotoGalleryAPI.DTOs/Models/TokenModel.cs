
namespace PhotoGalleryAPI.Services.JWT
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
    }
}
