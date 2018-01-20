using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{
    public interface IDataExporter
    {
        void Init(ExporterSetting setting);
        void Export(IEnumerable data, Stream outStream);
    }
}
