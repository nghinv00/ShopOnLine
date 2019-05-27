using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Core.Common
{
    public class GuidUnique
    {
        private static GuidUnique _instance = null;

        protected GuidUnique()
        {

        }
        public static GuidUnique getInstance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.

            if (_instance == null)
            {
                return new GuidUnique();
            }
            return _instance;
        }

        public string GenerateUnique()
        {
            return System.Guid.NewGuid().ToString("N");
        }
    }
}
