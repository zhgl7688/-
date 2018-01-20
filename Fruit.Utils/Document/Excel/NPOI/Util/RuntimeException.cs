﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fruit.Utils.NPOI.Util
{
    [Serializable]
    public class RuntimeException:Exception
    {
        public RuntimeException(string message)
            : base(message)
        {
        }
        public RuntimeException(Exception e)
            : base("", e)
        {
        }
        public RuntimeException(string exception, Exception ex)
            : base(exception, ex)
        {

        }
    }
}
