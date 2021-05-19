using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Bus;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain
{
    //Serviço de domínio: ele é cross aggregate
    //Deve conter ações/regras do negócio e as ações precisam ser da linguagem ubíqua
    public class EstoqueService : IEstoqueService, IDisposable
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatrHandler _bus;

        public EstoqueService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto == null)
            {
                return false; //podemos tbm lançar uma exception
            }

            if (!produto.PossuiEstoque(quantidade))
            {
                return false;
            }

            produto.DebitarEstoque(quantidade);

            //TODO: Parametrizar a quantidade de estoque baixo
            if (produto.QuantidadeEstoque < 10)
            {
                //avisar, mandar email, abrir chamado, realizar nova compra, etc.
                //Essa manipução é feita dentro da classe ProdutoEventHandler
                await _bus.PublicarEvento(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
                //TODO: aqui esse new infringe o Single Responsibility Principle? (acho que sim)
            }

            return await AtualizarProduto(produto);
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto == null)
            {
                return false; //podemos tbm lançar uma exception
            }

            produto.ReporEstoque(quantidade);

            return await AtualizarProduto(produto);
        }

        private async Task<bool> AtualizarProduto(Produto produto)
        {
            _produtoRepository.Atualizar(produto);
            return await _produtoRepository.UnitOfWork.Commit();
        }
        public void Dispose()
        {
            _produtoRepository.Dispose();
        }
    }
}
