using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;

namespace Cob.Umb.NavBuilder
{
    static class NavNodeExtensions
    {
        public static bool IsAncestor(this NavNode node, NavNode otherNode)
        {
            return node.Content.IsAncestor(otherNode.Content);
        }

        public static bool IsCurrent(this NavNode node)
        {
            return (node.Content.Id == node.Current.Id);
        }

        public static bool IsParent(this NavNode node)
        {
            return node.HasChildren;
        }

        public static bool IsOpen(this NavNode node)
        {
            return node.IsTraverseable;
        }

        public static bool IsClosed(this NavNode node)
        {
            return !node.IsOpen();
        }

        public static bool IsFirst(this NavNode node)
        {
            return node.Index == 0;
        }

        public static bool IsLast(this NavNode node)
        {
            return node.Parent == null || node.Index == node.Parent.ChildrenCount - 1;
        }

        public static bool IsEven(this NavNode node)
        {
            return node.Index % 2 == 0;
        }

        public static bool IsOdd(this NavNode node)
        {
            return !node.IsEven();
        }

        public static bool IsExternal(this NavNode node)
        {
            return node.Options.UseAbsoluteUrls ? !node.Url.StartsWith(NavNode.BaseUrl) : node.Url.StartsWith("http://");
        }
    }
}
