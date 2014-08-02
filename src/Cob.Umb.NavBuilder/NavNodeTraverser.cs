using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Cob.Umb.NavBuilder
{
    class NavNodeTraverser
    {
        private NavNode _root;

        public NavNodeTraverser(NavNode root)
        {
            _root = root;
        }

        public string Traverse()
        {
            return Traverse(_root);
        }

        private string Traverse(NavNode node)
        {
            StringBuilder menu = new StringBuilder();

            if (node.IsTraverseable)
            {
                menu.Append("<ul>");
                foreach (var child in node.Children)
                {
                    string name = HttpUtility.HtmlEncode(child.Name);
                    string nameOrImage = (child.HasImage) ? string.Format("<img src=\"{0}\" alt=\"{1}\" />", child.Image, name) : name;
                    string classes = (child.CssClasses == "") ? "" : string.Format(" class=\"{0}\"", child.CssClasses);
                    string target = (child.HasTarget) ? string.Format(" target=\"{0}\"", child.Target) : "";

                    menu.Append(string.Format("<li{0}><a href=\"{1}\"{2}>{3}</a>", classes, child.Url, target, nameOrImage));
                    menu.Append(Traverse(child));
                    menu.Append("</li>");
                }
                menu.Append("</ul>");
            }

            return menu.ToString();
        }
    }
}
