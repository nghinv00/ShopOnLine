using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Core.Common
{
    public class Format
    {

        /// <summary>
        /// Làm tròn số thập phân sang số nguyên dương và format sang định dạng tiền tệ Việt Nam
        /// </summary>
        /// <param name="PriceCurrent"></param>
        /// <returns>  </returns>
        public static string FormatDecimalToString(decimal PriceCurrent)
        {
            string html = string.Empty;

            if (PriceCurrent >= 0)
            {
                html = string.Format("{0:0,0}", PriceCurrent);
            }
            return html;
        }

        public static decimal SubStringDotInDecimal(decimal PriceCurrent)
        {
            string price = FormatDecimalToString(PriceCurrent);

            decimal.TryParse(price, out decimal _price);

            return _price;
        }
 
    }
}
