using System.Threading.Tasks;
using MediatR;

namespace QAP4.Domain.Core.Commands
{
    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IMediator _mediator;
        public CommandDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Send<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }
    }
}
