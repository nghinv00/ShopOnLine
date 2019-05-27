using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Core.ExModel
{
    public class TreeView
    {
        public TreeView()
        {

        }

        public string name { get; set; }
        public bool? open { get; set; }
        public bool? isParent { get; set; }

        public List<TreeView> children { get; set; } = new List<TreeView>();

        public string click { get; set; }
        public string id { get; set; }
        public string pId { get; set; }

    }
}
