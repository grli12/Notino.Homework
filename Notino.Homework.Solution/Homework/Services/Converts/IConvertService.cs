namespace Homework.Services.Converts
{
    public interface IConvertService
    {
        Task<string> ConvertAsync(string keyFrom, string keyTo, byte[] fileData, string targetPath);
    }
}
