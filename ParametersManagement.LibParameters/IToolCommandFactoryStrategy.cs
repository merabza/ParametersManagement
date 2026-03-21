using System.Threading;
using System.Threading.Tasks;

namespace ParametersManagement.LibParameters;

public interface IToolCommandFactoryStrategy
{
    string ToolCommandName { get; }

    ValueTask<IToolCommand?> CreateToolCommand(IParametersManager parametersManager,
        IFactoryStrategyParameters factoryStrategyParameters, CancellationToken cancellationToken = default);
}
