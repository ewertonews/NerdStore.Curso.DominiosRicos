using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Vendas.Domain
{
    public class ItemPedido : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        //EF Rel.
        public Pedido Pedido { get; private set; }

        public ItemPedido(
            Guid produtoId,
            string produtoNome,
            int quantidade,
            int valorUnitario)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        protected ItemPedido()
        {
        }

        internal void AssociarPedido(Guid pedidoId)
        {
            PedidoId = pedidoId;
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AumentarEstoque(int unidades)
        {
            Quantidade += unidades;
        }

        internal void AtualizarQuantidadeEstoque(int quantidade)
        {
            Quantidade = quantidade;
        }
    }
}
