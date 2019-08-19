using System.Threading.Tasks;

namespace QAP4.Domain.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task Send<T>(T command) where T : Command;
    }
}
