using DevExtremeAspNetCore.ViewModels;
using System.Drawing;

namespace DevExtremeAspNetCore.Repository
{
    public interface INPLRepository
    {
        Task<IEnumerable<NPLViewModel>> GetAllAsync();
        Task<NPLViewModel> GetByIdAsync(string id);
        Task AddAsync(NPLViewModel model);
        Task UpdateAsync(NPLViewModel model);
        Task DeleteAsync(string id);
    }
}
