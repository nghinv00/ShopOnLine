using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.UI.Tool
{
    public static class ExtensionsMethod
    {
        /// <summary>
        /// Kiểm tra chuỗi có phải là số tự nhiên hay không
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True nếu là số tự nhiên và False nếu ngược lại</returns>
        public static bool IsNumber(this string str)
        {
            int n;
            return int.TryParse(str, out n);
        }

        /// <summary>
        /// ngày thứ 7 sau ngày truyền vào gần nhất 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetSaturday(this DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            return new GregorianCalendar().AddDays(date, -((int)date.DayOfWeek) + 6);
        }

        #region Check value null or emmpty 
        /// <summary>
        /// Kiểm tra giá trị dữ liệu có null hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool checkNullOrEmpty(string entity)
        {
            if (string.IsNullOrWhiteSpace(entity))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
