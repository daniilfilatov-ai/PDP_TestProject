using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Domain.Interfaces;

public interface IFileReader 
{
   Task<string> ReadTextAsync(string filePath);
}
