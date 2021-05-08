using NerdStore.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        [Fact]
        public void New_DeveLancarExceptionParaNomeVazio()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Produto("", "descricao", true, 2, Guid.NewGuid(), DateTime.Now, "image", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Nome do produto n�o pode estar vazio", ex.Message);
        }

        [Fact]
        public void New_DeveLancarExceptionParaDescricaoVazia()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "", true, 2, Guid.NewGuid(), DateTime.Now, "image", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Descri��o do produto n�o pode estar vazio", ex.Message);
        }

        [Fact]
        public void New_DeveLancarExceptionParaImagemVazia()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descri��o", true, 2, Guid.NewGuid(), DateTime.Now, "", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Imagem do produto n�o pode estar vazio", ex.Message);
        }

        [Fact]
        public void New_DeveLancarExceptionParaIdCategoriaGuidVazio()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descri��o", true, 2, Guid.Empty, DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo CategoriaId do produto n�o pode estar vazio", ex.Message);
        }

        [Fact]
        public void New_DeveLancarExceptionParaValorMenorQueMinimo()
        {
            var ex = Assert.Throws<DomainException>(() =>
               new Produto("Nome", "Descri��o", true, 0.5M, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
           );

            Assert.Equal("O campo Valor do produto n�o pode ser menor que 1", ex.Message);
        }
    }
}
