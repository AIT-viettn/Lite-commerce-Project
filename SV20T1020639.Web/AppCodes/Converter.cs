using Newtonsoft.Json;
using SV20T1020639.DomainModels;
using System.Globalization;

namespace SV20T1020639.Web
{
    public static class Converter
    {
        /// <summary>
        /// chuyển chuổi s sang giá trị kiểu datetime theo các format đã được quy định
        /// chuyển về null nếu chuyển không thành cồng
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);

            }
            catch
            {
                return null;
            }
        }
        

    }
}
