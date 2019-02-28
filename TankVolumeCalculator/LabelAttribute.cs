using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankVolumeCalculator
{
    [AttributeUsage(AttributeTargets.Property)]
    class LabelAttribute : Attribute
    {
        private readonly string label;

        public String Label
        {
            get { return this.label; }
        }

        public LabelAttribute(string label)
        {
            this.label = label;
        }
    }
}
