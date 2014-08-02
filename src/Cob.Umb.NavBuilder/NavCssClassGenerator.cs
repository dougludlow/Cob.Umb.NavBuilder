using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cob.Umb.NavBuilder
{
    class NavCssClassGenerator
    {
        private NavNode _node;

        public NavCssClassGenerator(NavNode node)
        {
            this._node = node;
        }

        internal string Generate()
        {
            string output = string.Empty;
            var classes = new HashSet<string>();

            if (_node.IsCurrent)
                classes.Add("selected");

            if (_node.HasChildren)
            {
                classes.Add("parent");

                if (_node.IsOpen)
                    classes.Add("open");
                else
                    classes.Add("closed");
            }

            if (_node.IsFirst)
                classes.Add("first");

            if (_node.IsLast)
                classes.Add("last");

            if (_node.IsEven)
                classes.Add("even");
            else
                classes.Add("odd");

            if (_node.HasImage)
                classes.Add("image");

            if (classes.Count > 0)
                output = string.Join(" ", classes);

            return output;
        }
    }
}
