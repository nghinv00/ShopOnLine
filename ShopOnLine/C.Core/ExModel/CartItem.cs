using C.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C.Core.ExModel
{
    [Serializable]
    public class CartItem
    {

        public shProduct Product { get; set; }
        public int Quantity { get; set; }
        public string SectionGuid { get; set; }
        public string SizeGuid { get; set; }
        

    }
}