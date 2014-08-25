using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cob.Umb.NavBuilder
{
    public class NavBuilder
    {
        NavNode root;
        NavNode current;
        NavBuilderOptions options;

        public NavBuilder(int id) : this(id, new NavBuilderOptions()) { }

        public NavBuilder(int id, NavBuilderOptions options)
        {
            this.current = new NavNode(id, options);
            this.root = current.Root;
            this.options = options;
        }

        public string Build()
        {
            return new NavNodeTraverser(this.root).Traverse();
        }

        public NavBuilder MaxLevel(int level)
        {
            this.options.MaxLevel = level;
            return this;
        }

        public NavBuilder IncludeHome()
        {
            this.IncludeHome(true);
            return this;
        }

        public NavBuilder IncludeHome(bool flag)
        {
            this.options.IncludeHomeNode = flag;
            return this;
        }

        public NavBuilder ExcludeType(params string[] aliases)
        {
            if (aliases != null)
                this.options.ExcludedTypes.AddRange(aliases.Where(a => !string.IsNullOrWhiteSpace(a)));
            return this;
        }

        public NavBuilder ExcludeChildrenOfType(params string[] aliases)
        {
            if (aliases != null)
                this.options.ExcludedParentTypes.AddRange(aliases.Where(a => !string.IsNullOrWhiteSpace(a)));
            return this;
        }

        public NavBuilder ShowAll()
        {
            this.ShowAll(true);
            return this;
        }

        public NavBuilder ShowAll(bool flag)
        {
            this.options.ShowAllNodes = flag;
            return this;
        }

        public NavBuilder UseAbsoluteUrls()
        {
            this.UseAbsoluteUrls(true);
            return this;
        }

        public NavBuilder UseAbsoluteUrls(bool flag)
        {
            this.options.UseAbsoluteUrls = flag;
            return this;
        }

        public NavBuilder AddRootCssClasses(params string[] classes)
        {
            if (classes != null)
                this.options.ExcludedParentTypes.AddRange(classes.Where(a => !string.IsNullOrWhiteSpace(a)));
            return this;
        }
    }
}
