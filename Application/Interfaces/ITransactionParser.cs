using System.Collections.Generic;
using PDP_TestProject.Domain.Models;

namespace PDP_TestProject.Application.Interfaces;

public interface ITransactionParser
{
    IEnumerable<Transaction> Parse(string rawData);
}
