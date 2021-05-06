using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; private set; }
        //Aqui o objeto de valor Dimensoes está agregando valor ao Produto
        public Dimensoes Dimensoes { get; private set; }

        public Produto(
            string nome,
            string descricao,
            bool ativo,
            decimal valor,
            Guid categoriaId,
            DateTime dataCadastro,
            string imagem, 
            Dimensoes dimensoes)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            CategoriaId = categoriaId;
            DataCadastro = dataCadastro;
            Imagem = imagem;           
            Dimensoes = dimensoes;

            Validar();
        }

        //TODO: Qual a vantagem de alterar as propriedades do objeto dessa forma?
        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string novaDescricao)
        {
            //Valida algo
            Descricao = novaDescricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSePreenchido(Nome, "O campo Nome do produto não pode estar vazio");
            Validacoes.ValidarSePreenchido(Descricao, "O campo Descrição do produto não pode estar vazio");
            Validacoes.ValidarSePreenchido(Imagem, "O campo Imagem do produto não pode estar vazio");
            Validacoes.ValidarSeDiferente(CategoriaId, Guid.Empty, "O campo CategoriaId do produto não pode estar vazio");
            Validacoes.ValidarSeMaiorIgualMinimo(Valor, 1, "O campo Valor do produto não pode ser menor ou igual a 0");
        }   
    }
}
