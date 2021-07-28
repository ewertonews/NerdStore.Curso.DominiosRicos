using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<ItemPedido> _itensPedido;
        public IReadOnlyCollection<ItemPedido> ItensPedido => _itensPedido;
        //EF Relationship
        public virtual Voucher Voucher { get; private set; }

        public Pedido(Guid clientId, bool voucherUtilizado, decimal desconto, decimal valorTotal)
        {
            ClientId = clientId;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;
            _itensPedido = new List<ItemPedido>();
        }

        public Pedido()
        {
            _itensPedido = new List<ItemPedido>();
        }

        public void AplicarVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorPedido();
        }

        public void CalcularValorPedido()
        {
            ValorTotal = ItensPedido.Sum(p => p.CalcularValor());
            CalcularValorTotalDeconto();
        }

        public void CalcularValorTotalDeconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if (Voucher.TipoDescontoVouncher == TipoDescontoVouncher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else if (Voucher.ValorDesconto.HasValue)
            {
                desconto = Voucher.ValorDesconto.Value;
                valor -= desconto;
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        public void AdicionarItem(ItemPedido item)
        {
            if (!item.EhValido()) return;

            item.AssociarPedido(Id);

            //Aqui é caso o cliente decida adicionar mais um item que já está no carrinho
            // e evita que dois itens iguais seja adicionado ao pedido.
            if (ItemPedidoExistente(item))
            {
                ItemPedido itemExistente = _itensPedido.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                //ao achar o pedido, a quantidade desse item no pedido será incrementada
                itemExistente.AdicionarUnidades(item.Quantidade);
                //o item a ser inserido vai passar a ser o item já existente
                item = itemExistente;
                //e o item existente vai ser removido, para que lá na frente seja adicionado novamente ao pedido 
                //depois de calcular o valor total desses itens  (próximas duas linhas depois do if)
                _itensPedido.Remove(itemExistente);
            }

            item.CalcularValor();
            _itensPedido.Add(item);

            CalcularValorPedido();
        }

        public void RemoverItem(ItemPedido item)
        {
            if(!item.EhValido()) return;

            ItemPedido itemExistente = ItensPedido.FirstOrDefault(p => p.ProdutoId  == item.ProdutoId);

            if (itemExistente == null)
            {
                throw new DomainException("O item não pertence ao pedido");
            }
            _itensPedido.Remove(itemExistente);

            CalcularValorPedido();
        }

        public void AtualizarItem(ItemPedido item)
        {
            if (!item.EhValido()) return;

            item.AssociarPedido(Id);

            ItemPedido itemExistente = ItensPedido.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            if (itemExistente == null)
            {
                throw new DomainException("O item não pertence ao pedido");
            }

            _itensPedido.Remove(itemExistente);
            _itensPedido.Add(item);

            CalcularValorPedido();
        }

        //Esse método é apenas uma ponte para o metodo do item, pois 
        //esse método (do item) precisa ser manipulado através da raiz da agregação
        public void AtualizarUnidades(ItemPedido item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        public bool ItemPedidoExistente(ItemPedido item)
        {
            return _itensPedido.Any(p => p.ProdutoId == item.ProdutoId);
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public void IniciarPedido()
        {
            PedidoStatus = PedidoStatus.Iniciado;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = PedidoStatus.Pago;
        }
        
        public void CancelarPedido()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido()
                {
                    ClientId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}
