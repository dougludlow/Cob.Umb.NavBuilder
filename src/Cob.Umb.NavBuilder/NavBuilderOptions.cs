using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cob.Umb.NavBuilder
{
    public class NavBuilderOptions
    {
        public const int DefaultMaxLevel = 4;

        private int maxLevel = DefaultMaxLevel;

        public NavBuilderOptions()
        {
            ExcludedTypes = new List<string>();
            ExcludedParentTypes = new List<string>();
            RootCssClasses = new List<string>();
            IncludeHomeNode = false;
            ShowAllNodes = false;
            UseAbsoluteUrls = false;
        }

        public List<string> ExcludedTypes { get; set; }
        public List<string> ExcludedParentTypes { get; set; }
        public List<string> RootCssClasses { get; set; }
        public bool IncludeHomeNode { get; set; }
        public bool ShowAllNodes { get; set; }
        public bool UseAbsoluteUrls { get; set; }

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
    }
}
