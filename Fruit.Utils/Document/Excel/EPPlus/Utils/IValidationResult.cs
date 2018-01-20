using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fruit.Utils.EPPlus.Utils
{
    public interface IValidationResult
    {
        void IsTrue();
        void IsFalse();
    }
}
