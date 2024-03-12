using System.Xml.Linq;

namespace SV20T1020639.Web
{
    /// <summary>
    /// Thông tin về nhóm / quyền
    /// </summary>
    public class WebUserRole
    {
        public WebUserRole(string name, string description)
        {
            Name = name;
            Description = description;
        }
        /// <summary>
        /// Tên/Ký hiệu quyền
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả 
        /// </summary>
        public string Description { get; set; }


    }
}
