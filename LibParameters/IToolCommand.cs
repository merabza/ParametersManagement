using System.Threading.Tasks;
using System.Threading;

namespace LibParameters;

public interface IToolCommand
{
    IParameters Par { get; }
    string? Description { get; }
    Task<bool> Run(CancellationToken cancellationToken);
}