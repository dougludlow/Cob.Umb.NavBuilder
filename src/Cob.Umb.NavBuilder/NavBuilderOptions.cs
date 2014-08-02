using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cob.Umb.NavBuilder
{
    public class NavBuilderOptions
    {
        public const int DefaultMaxLevel = 4;

        List<string> excludedTypes = new List<string>();
        List<string> excludedParentTypes = new List<string>();
        private bool includeHomeNode = false;
        private int maxLevel = DefaultMaxLevel;
        private bool showAllNodes = false;
        private bool useAbsoluteUrls = false;

        public List<string> ExcludedTypes
        {
            get
            {
                return this.excludedTypes;
            }
            set
            {
                this.excludedTypes = value;
            }
        }

        public List<string> ExcludedParentTypes
        {
            get
            {
                return this.excludedParentTypes;
            }
            set
            {
                this.excludedParentTypes = value;
            }
        }

        public bool IncludeHomeNode
        {
            get
            {
                return this.includeHomeNode;
            }
            set
            {
                this.includeHomeNode = value;
            }
        }

        public int MaxLevel
        {
            get
            {
                return this.maxLevel;
            }
            set
            {
                this.maxLevel = (value > DefaultMaxLevel || value < 1) ? DefaultMaxLevel : value;
            }
        }

        public bool ShowAllNodes
        {
            get
            {
                return this.showAllNodes;
            }
            set
            {
                this.showAllNodes = value;
            }
        }

        public bool UseAbsoluteUrls
        {
            get
            {
                return this.useAbsoluteUrls;
            }
            set
            {
                this.useAbsoluteUrls = value;
            }
        }
    }
}
