using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainingV1
{
    public class SomeProcess
    {
        internal async Task<string> LongAsyncProcess(IProgress<int> _prog, CancellationToken ct, Action action,int delayTime)
        {
            string startValue = "Long text wchich is in long async function";
            StringBuilder sb = new StringBuilder();
            ct.Register(action);
            foreach (var item in startValue)
            {
                ct.ThrowIfCancellationRequested();
                sb.Append(item);
                _prog?.Report((int)(((decimal)sb.Length * 100) / ((decimal)startValue.Length)));
                await Task.Delay(delayTime);
            }
            return sb.ToString();
        }
    }
}
