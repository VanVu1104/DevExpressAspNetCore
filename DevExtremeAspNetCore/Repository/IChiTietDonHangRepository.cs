using DevExtremeAspNetCore.ViewModels;
using System.Drawing;

namespace DevExtremeAspNetCore.Repository
{
    public interface IChiTietDonHangRepository
    {
        Task<PagedResult<ChiTietDonHangViewModel>> GetAllAsync(int pageNumber, int pageSize);
        Task<List<string>> GetAllSize();
    }
}
