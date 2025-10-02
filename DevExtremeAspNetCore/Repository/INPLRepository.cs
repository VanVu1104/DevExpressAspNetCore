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
        Task<List<string>> GetColorsAsync(string term);
        Task<List<string>> GetUnitsAsync();
        Task<List<string>> GetTypesAsync();
    }
}
