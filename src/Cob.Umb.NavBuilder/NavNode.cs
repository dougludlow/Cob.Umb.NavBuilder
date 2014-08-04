using RJP.MultiUrlPicker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Cob.Umb.NavBuilder
{
    class NavNode
    {
        private static string baseUrl;
        public static string BaseUrl
        {
            get
            {
                if (baseUrl == null)
                    baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                return baseUrl;
            }
        }

        public static UmbracoHelper UmbracoHelper = new UmbracoHelper(UmbracoContext.Current);

        public IPublishedContent Content { get; private set; }
        public NavBuilderOptions Options { get; private set; }
        public IPublishedContent Current { get; private set; }
        public int Index { get; private set; }

        private NavNode root;
        private NavNode parent;
        private IEnumerable<NavNode> children;
        private int? childrenCount;
        private string name;
        private string target;
        private string url;
        private string image;
        private bool? isVisible;
        private bool? isNavigationExpanded;
        private bool? isAllNavigationExpanded;

        #region Constructors

        public NavNode(int id, NavBuilderOptions options)
        {
            var content = UmbracoHelper.TypedContent(id);
            Init(content, content, 0, null, null, options);
        }

        public NavNode(IPublishedContent content, NavBuilderOptions options)
        {
            Init(content, content, 0, null, null, options);
        }

        private NavNode(IPublishedContent content, IPublishedContent current, NavBuilderOptions options)
        {
            Init(content, current, 0, null, null, options);
        }

        private NavNode(IPublishedContent content, int index, NavNode parent)
        {
            Init(content, parent.Current, index, parent, null, parent.Options);
        }

        private NavNode(IPublishedContent content, int index, NavNode parent, IEnumerable<NavNode> children)
        {
            Init(content, parent.Current, index, parent, children, parent.Options);
        }

        private void Init(IPublishedContent content, IPublishedContent current, int index, NavNode parent, IEnumerable<NavNode> children, NavBuilderOptions options)
        {
            Content = content;
            Index = index;
            Current = current;
            Options = options;
            if (parent != null) this.parent = parent;
            if (children != null) this.children = children;
        }

        #endregion

        public NavNode Root
        {
            get
            {
                if (root == null)
                {
                    var ancestorOrSelf = Content.AncestorOrSelf(1);

                    if (ancestorOrSelf.Id == this.Id)
                        root = this;
                    else
                        root = new NavNode(ancestorOrSelf, Current, Options);

                }
                return root;
            }
        }

        public NavNode Parent
        {
            get
            {
                if (parent == null)
                {
                    if (Content.Level == 1 && Content.Parent == null)
                        parent = new NavNode(Content.Parent, Current, Options);
                }
                return parent;
            }
        }

        public IEnumerable<NavNode> Children
        {
            get
            {
                if (children == null)
                {
                    if (Options.ExcludedParentTypes.Any(t => t == Content.DocumentTypeAlias))
                        children = Enumerable.Empty<NavNode>();
                    else
                        children = WrapChildren(Content.Children.Cast<IPublishedContent>()
                            .Where(n => n.IsVisible() && !Options.ExcludedTypes.Any(t => t == n.DocumentTypeAlias)));
                }

                return children;
            }
        }

        private IEnumerable<NavNode> WrapChildren(IEnumerable<IPublishedContent> nodes)
        {
            int index = 0;

            if (this.IsHome && Options.IncludeHomeNode)
                yield return new NavNode(Content, index++, this, Enumerable.Empty<NavNode>());

            foreach (var node in nodes)
                yield return new NavNode(node, index++, this);
        }

        public IPublishedContent IPublishedContent
        {
            get
            {
                return Content;
            }
        }

        public int Id
        {
            get
            {
                return Content.Id;
            }
        }

        public int Level
        {
            get
            {
                return Content.Level;
            }
        }

        public string Name
        {
            get
            {
                if (name == null)
                {
                    if (Content.HasValue("altName"))
                        name = Content.GetPropertyValue<string>("altName");
                    else
                        name = Content.Name;
                }
                return name;
            }
        }

        public string Image
        {
            get
            {
                if (image == null)
                {
                    if (Content.HasValue("altName"))
                    {
                        var navImage = Content.GetPropertyValue<string>("navImage");
                        int id;
                        if (int.TryParse(navImage, out id))
                        {
                            var media = UmbracoHelper.TypedMedia(id);
                            if (media.Id != 0)
                                image = media.GetPropertyValue<string>("umbracoFile");
                        }
                    }
                    else
                    {
                        image = "";
                    }
                }
                return image;
            }
        }

        public string Url
        {
            get
            {
                if (url == null)
                {
                    url = UmbracoHelper.NiceUrl(Content.Id);

                    if (Content.DocumentTypeAlias == "Link")
                    {
                        if (Content.HasValue("urlPicker"))
                        {
                            url = "#";
                            var urlPicker = Content.GetPropertyValue<MultiUrls>("urlPicker").FirstOrDefault();

                            if (urlPicker != null)
                            {
                                switch (urlPicker.Type)
                                {
                                    case LinkType.Content:
                                        if (urlPicker.Id.HasValue)
                                            url = UmbracoHelper.NiceUrl(urlPicker.Id.Value);
                                        break;

                                    case LinkType.Media:
                                        if (urlPicker.Id.HasValue)
                                            url = UmbracoHelper.TypedMedia(urlPicker.Id).GetPropertyValue<string>("umbracoFile");
                                        break;

                                    case LinkType.External:
                                        url = urlPicker.Url;
                                        break;
                                }
                            }
                        }
                    }

                    if (Options.UseAbsoluteUrls && !url.StartsWith("http"))
                        url = BaseUrl + url;
                }

                return url;
            }
        }

        public string Target
        {
            get
            {
                if (this.target == null)
                {
                    if (Content.DocumentTypeAlias == "Link" && Content.HasValue("urlPicker"))
                    {
                        var urlPicker = Content.GetPropertyValue<MultiUrls>("urlPicker").FirstOrDefault();
                        if (urlPicker != null)
                            this.target = urlPicker.Target;
                    }
                }
                return this.target;
            }
        }

        public string CssClasses
        {
            get
            {
                return new NavCssClassGenerator(this).Generate();
            }
        }

        public int ChildrenCount
        {
            get
            {
                if (this.childrenCount == null)
                    this.childrenCount = this.Children.Count();
                return this.childrenCount.Value;
            }
        }

        #region Helper Properties

        public bool HasChildren
        {
            get
            {
                return ChildrenCount > 0;
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Image);
            }
        }

        public bool HasTarget
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Target);
            }
        }


        public bool IsVisible
        {
            get
            {
                if (!this.isVisible.HasValue)
                {
                    this.isVisible = Content.IsVisible();
                }
                return this.isVisible.Value;
            }
        }

        public bool IsTraverseable
        {
            get
            {
                return (Options.ShowAllNodes
                    || Content.IsAncestorOrSelf(Current)
                    || this.IsNavigationExpanded
                    || this.IsAllNavigationExpanded)
                    && (this.HasChildren && this.Level < Options.MaxLevel);
            }
        }

        public bool IsHome
        {
            get
            {
                return Level == 1;
            }
        }

        public bool IsNavigationExpanded
        {
            get
            {
                if (!this.isNavigationExpanded.HasValue)
                    this.isNavigationExpanded = Content.GetPropertyValue<bool>("expandNavigation");
                return this.isNavigationExpanded.Value;
            }
        }

        public bool IsAllNavigationExpanded
        {
            get
            {
                if (!this.isAllNavigationExpanded.HasValue)
                    this.isAllNavigationExpanded = Content.GetPropertyValue<bool>("expandAllNavigation", true);
                return this.isAllNavigationExpanded.Value;
            }
        }

        #endregion
    }
}
