namespace PDP_TestProject.Interfaces
{

    public interface IDataSource<TInput>
    {
        IEnumerable<TInput> ReadData(string sourcePath);
    }

    public interface IDataFormatter<TInput, TOutput>
    {
        TOutput Format(TInput inputData);
    }

    public interface IDataRepository<TOutput>
    {
        void SaveData(IEnumerable<TOutput> data);
    }
}