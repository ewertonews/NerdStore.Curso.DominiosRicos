using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMapper _mapper;

        public ProdutoAppService(
            IProdutoRepository produtoRepository,
            IMapper mapper,
            IEstoqueService estoqueService)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _estoqueService = estoqueService;
        }

        public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Adicionar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Atualizar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<IEnumerable<CategoriaViewModel>> ObterCategorias()
        {
            var categorias = await _produtoRepository.ObterCategorias();
            return _mapper.Map<IEnumerable<CategoriaViewModel>>(categorias);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
        {
            var produtoDaCategoria = await _produtoRepository.ObterPorCategoria(codigo);
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(produtoDaCategoria);
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var produtoQueTemId = await _produtoRepository.ObterPorId(id);
            return _mapper.Map<ProdutoViewModel>(produtoQueTemId);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var produtos = await _produtoRepository.ObterTodos();
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
        }

        public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
        {
            var debitouDoEstoque = await _estoqueService.DebitarEstoque(id, quantidade);
            if (!debitouDoEstoque)
            {
                throw new DomainException("Falha ao debitar estoque");
            }

            var produtoComEstoqueAtualizado = await _produtoRepository.ObterPorId(id);
            return _mapper.Map<ProdutoViewModel>(produtoComEstoqueAtualizado);
        }

        public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
        {
            var reposEstoque = await _estoqueService.ReporEstoque(id, quantidade);
            if (!reposEstoque)
            {
                throw new DomainException("Falha ao repor estoque");
            }
            var produtoComEstoqueAtualizado = await _produtoRepository.ObterPorId(id);
            return _mapper.Map<ProdutoViewModel>(produtoComEstoqueAtualizado);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
