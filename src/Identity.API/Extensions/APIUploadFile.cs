using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Extensions
{

    public static class APIUploadFile
    {
        public static WebApplication MapAPI_File(this WebApplication app)
        {
            app
                .MapPost(@"api/v1/files/upload", InternalMethods.UploadFileHandler)
                .WithTags(@"Upload");
            return app;
        }

        public static class InternalMethods
        {
            public static CloudinaryService? cloudinaryService;
            public static async Task<IResult> UploadFileHandler(
                [FromServices] IConfiguration configuration,
                HttpContext httpContext)
            {
                try
                {
                    IFormFileCollection files = httpContext.Request.Form.Files;
                    if (files == null || !files.Any())
                    {
                        return Results.BadRequest("No files selected");
                    }
                    IEnumerable<IFormFile> validFiles = files.Where(file => (file != null && file.Length > 0));
                    if (!validFiles.Any())
                    {
                        return Results.BadRequest("No valid files upload");
                    }
                    cloudinaryService = new CloudinaryService(configuration);
                    List<string> imageUrls = new();
                    foreach (IFormFile file in validFiles)
                    {
                        imageUrls.Add(await cloudinaryService.UploadImage(file));
                    }
                    return Results.Ok(new
                    {
                        ImageUrls = imageUrls
                    });
                }
                catch
                {
                    return Results.StatusCode(500);
                }
            }
        }
    }
}
