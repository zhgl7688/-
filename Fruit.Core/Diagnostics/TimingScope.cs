using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Diagnostics
{
    /// <summary>
    /// 用于表示一个计时范围，用于查看代码块的运行时间
    /// </summary>
    public class TimingScope : IDisposable
    {
        private Stopwatch watcher;

        public TimingScope([CallerMemberName] string name = null)
        {
            this.Name = name;
            this.watcher = new Stopwatch();

            Trace.TraceInformation("[{0}] 开始于 {1}", name, DateTime.Now);
            watcher.Start();
        }

        public string Name { get; private set; }

        public void Dispose()
        {
            watcher.Stop();
            Trace.TraceInformation("[{0}] 结束于 {1} ，用时 {2}ms / {3} ticks", Name, DateTime.Now, watcher.ElapsedMilliseconds, watcher.ElapsedTicks);
        }
    }
}
