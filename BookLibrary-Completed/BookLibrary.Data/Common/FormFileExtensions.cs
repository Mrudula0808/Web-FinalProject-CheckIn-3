using Microsoft.AspNetCore.Http;

namespace BookLibrary.Data.Common
{
    public static class FormFileExtensions
    {
        public static byte[] GetFileData(this IFormFile formFile)
        {
            var stream = new MemoryStream();
            formFile.CopyTo(stream);
            var data = stream.ToArray();
            stream.Dispose();
            return data;
        }

        public static bool IsPdfFile(this IFormFile? formFile)
        {
            if (formFile == null) return false;
            return formFile.ContentType == "application/pdf";
        }

        public static bool IsJpegImageFile(this IFormFile? formFile)
        {
            if (formFile == null) return false;
            return formFile.ContentType == "image/jpe" || formFile.ContentType == "image/jpg" || formFile.ContentType == "image/jpeg";
        }
    }
}
