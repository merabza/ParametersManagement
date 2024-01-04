using System.Threading;
using System.Threading.Tasks;

namespace LibParameters;

public interface IToolCommand
{
    IParameters Par { get; }
    string? Description { get; }
    Task<bool> Run(CancellationToken cancellationToken);
}