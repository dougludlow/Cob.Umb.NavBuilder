using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cob.Umb.NavBuilder
{
    class NavCssClassGenerator
    {
        private NavNode node;

        public NavCssClassGenerator(NavNode node)
        {
            this.node = node;
        }

        internal string Generate()
        {
            string output = string.Empty;
            var classes = new HashSet<string>();

            if (node.IsCurrent())
                classes.Add("selected");

            if (node.HasChildren)
            {
                classes.Add("parent");

                if (node.IsOpen())
                    classes.Add("open");
                else
                    classes.Add("closed");
            }

            if (node.IsFirst())
                classes.Add("first");

            if (node.IsLast())
                classes.Add("last");

            if (node.IsEven())
                classes.Add("even");
            else
                classes.Add("odd");

            if (node.HasImage)
                classes.Add("image");

            if (classes.Count > 0)
                output = string.Join(" ", classes);

            return output;
        }
    }
}
