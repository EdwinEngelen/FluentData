using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FluentData.Atrributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute:Attribute
    {
    }
}
