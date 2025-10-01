using DevExtremeAspNetCore.ViewModels;
using System.Drawing;

namespace DevExtremeAspNetCore.Repository
{
    public interface INPLRepository
    {
        Task<List<NPLViewModel>> GetAllAsync();
    }
}
