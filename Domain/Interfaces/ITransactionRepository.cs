using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Domain.Interfaces;

public interface ITransactionRepository
{
    Task SaveAsync(IEnumerable<Transaction> transactions);
}
