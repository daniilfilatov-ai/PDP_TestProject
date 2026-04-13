namespace PDP_TestProject.Application.Interfaces;

public interface IFileReader 
{
   Task<string> ReadTextAsync(string filePath);
}
