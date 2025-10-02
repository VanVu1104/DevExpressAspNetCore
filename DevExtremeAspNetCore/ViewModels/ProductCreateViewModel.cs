namespace DevExtremeAspNetCore.ViewModels
{
    public class ProductCreateViewModel
    {
        public int IDPro { get; set; }
        public string TenPro { get; set; }
        public List<VariantCreateViewModel> Variants { get; set; }
    }
}
