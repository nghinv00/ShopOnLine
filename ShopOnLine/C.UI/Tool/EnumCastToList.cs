using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.UI.Tool
{
    public class EnumCastToList
    {
        public static IEnumerable<T> GetValuesEnum<T>()
        {
            return Enum.GetNames(typeof(T)).Cast<T>();
        }

        public static IEnumerable<T> GetNamesEnum<T>()
        {
            return Enum.GetNames(typeof(T)).Cast<T>();
        }
    }
   

}
