using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetProducts();
        Task<List<ColorDto>> GetColors();
        Task<List<SizeDto>> GetSizes();
    }
}
