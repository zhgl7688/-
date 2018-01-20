﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false, Inherited=true)]
    public class NoColumnAttribute : Attribute
    {
    }
}
