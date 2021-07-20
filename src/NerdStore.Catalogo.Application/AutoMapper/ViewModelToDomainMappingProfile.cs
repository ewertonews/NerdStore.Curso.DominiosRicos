using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProdutoViewModel, Produto>()
                .ConstructUsing(pvm =>
                    new Produto(pvm.Nome, pvm.Descricao, pvm.Ativo,
                        pvm.Valor, pvm.CategoriaId, pvm.DataCadastro,
                        pvm.Imagem, new Dimensoes(pvm.Altura, pvm.Largura, pvm.Profundidade)));

            CreateMap<CategoriaViewModel, Categoria>()
                .ConstructUsing(cvm => new Categoria(cvm.Nome, cvm.Codigo));
        }
    }
}
