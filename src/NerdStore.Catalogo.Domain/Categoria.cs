using NerdStore.Core.DomainObjects;
using System.Collections.Generic;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        //EF Requirement - Produtos de uma categoria
        public ICollection<Produto> Produtos { get; set; }

        protected Categoria()
        {
        }

        //fim EF Requirement

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            Validacoes.ValidarSePreenchido(Nome, "O campo Nome da categoria não pode estar vazio");
            Validacoes.ValidarSeDiferente(Codigo, 0, "O Campo Código nõa pode ser 0");
        }
    }
}
