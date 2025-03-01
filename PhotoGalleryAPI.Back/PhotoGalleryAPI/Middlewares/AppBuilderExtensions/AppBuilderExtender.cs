namespace PhotoGalleryAPI.Middlewares.AppBuilderExtensions
{
    public static class AppBuilderExtender
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
