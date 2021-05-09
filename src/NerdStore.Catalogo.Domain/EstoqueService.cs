using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain
{
    //Serviço de domínio: ele é cross aggregate
    public class EstoqueService : IEstoqueService, IDisposable
    {
        private readonly IProdutoRepository _produtoRepository;

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
