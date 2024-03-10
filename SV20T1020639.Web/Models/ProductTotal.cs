using SV20T1020639.DomainModels;

namespace SV20T1020639.Web.Models
{
    public class ProductTotal
    {
        public Product Product { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
    }
}
