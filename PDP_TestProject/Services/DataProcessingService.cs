using PDP_TestProject.Interfaces;

namespace PDP_TestProject.Services
{
    public class DataProcessingService<TInput, TOutput>(
        IDataSource<TInput> dataSource,
        IDataFormatter<TInput, TOutput> dataFormatter,
        IDataRepository<TOutput> dataRepository)
    {
        public void Process(string sourcePath)
        {
            var rawDataList = dataSource.ReadData(sourcePath);
            var processedDataList = new List<TOutput>();

            foreach (var rawData in rawDataList)
            {
                var formattedRecord = dataFormatter.Format(rawData);
                if (formattedRecord != null)
                {
                    processedDataList.Add(formattedRecord);
                }
            }
            if (processedDataList.Count > 0)
            {
                dataRepository.SaveData(processedDataList);
            }
        }
    }
}
