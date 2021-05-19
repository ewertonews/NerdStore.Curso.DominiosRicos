using MediatR;
using NerdStore.Core.Messages;
using System;
using System.Threading.Tasks;

namespace NerdStore.Core.Bus
{
    //Essa classe é um wrapper pro MediatR
    //O mediator é um Bus InMemoery, e pode ser usado como uma interface pro meu Bus normal
    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;

        public MediatrHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }
}
