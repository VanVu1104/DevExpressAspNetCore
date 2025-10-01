using DevExtremeAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetAll();
        Task<List<ColorDto>> GetColorsByProduct(int idPro);
        Task<List<SizeDto>> GetSizesByProductAndColor(int idPro, int idColor);
    }
}
