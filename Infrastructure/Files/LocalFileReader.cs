using PDP_TestProject.Domain.Interfaces;

namespace PDP_TestProject.Infrastructure.Files
{
    public class LocalFileReader : IFileReader
    {
        public async Task<string> ReadTextAsync (string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }
            return await File.ReadAllTextAsync(filePath);
        }
    }
}
