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
    }
}
