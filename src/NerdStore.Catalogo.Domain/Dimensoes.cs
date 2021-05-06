using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain
{
    //Objeto de valor
    public class Dimensoes
    {
        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            ValidarPropriedades(altura, largura, profundidade);

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        private static void ValidarPropriedades(decimal altura, decimal largura, decimal profundidade)
        {
            Validacoes.ValidarSeMaiorIgualMinimo(altura, 1, "O campo altura não pode ser menor que 1");
            Validacoes.ValidarSeMaiorIgualMinimo(largura, 1, "O campo largura não pode ser menor que 1");
            Validacoes.ValidarSeMaiorIgualMinimo(profundidade, 1, "O campo profundidade não pode ser menor que 1");
        }
    }
}
