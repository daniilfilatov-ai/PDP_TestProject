using PDP_TestProject.Interfaces;

namespace PDP_TestProject.Implementations
{
    public class FileDataSource : IDataSource<string>
    {
        public IEnumerable<string> ReadData(string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException($"The file at path {sourcePath} was not found.");
            }
            
            string content = File.ReadAllText(sourcePath);
            
            var transactions = content.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries);

            foreach (var transactionBlock in transactions)
            {
                yield return transactionBlock;
            }
        }
    }
}
