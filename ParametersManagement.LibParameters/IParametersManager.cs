using System.Threading;
using System.Threading.Tasks;

namespace ParametersManagement.LibParameters;

public interface IParametersManager
{
    IParameters Parameters { get; set; }
    ValueTask Save(IParameters parameters, string message, string? saveAsFilePath = null, CancellationToken cancellationToken = default);
}
