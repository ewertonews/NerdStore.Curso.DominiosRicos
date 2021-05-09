using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.DomainObjects
{
    //Entender: https://medium.com/@martinstm/unit-of-work-net-core-652f9b6cf894
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
