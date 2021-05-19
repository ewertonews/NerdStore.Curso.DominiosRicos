using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        //TODO: refazer e seguir sugestão de Balta.io: https://www.youtube.com/watch?v=HdsRpSK4PUg
        IUnitOfWork UnitOfWork { get; }
    }
}
