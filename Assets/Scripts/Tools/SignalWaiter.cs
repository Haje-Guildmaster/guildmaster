using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; 

namespace GuildMaster.Tools
{
    public class SignalWaiter
    {
        public int WaitersCount => _tcsSet.Count;
        public bool WaiterExist => WaitersCount > 0;

        /// <summary>
        /// 이 객체에 Wait하고 있던 모든 코드가 다음으로 넘어가도록 하고 그 수를 반환함. 
        /// </summary>
        /// <returns></returns>
        public int Signal()
        {
            var ret = WaitersCount;
            foreach (var tcs in _tcsSet)
            {
                tcs.TrySetResult(true);
            }

            _tcsSet.Clear();
            return ret;
        }

        /// <summary>
        /// Signal이 올 때까지 기다림.
        /// </summary>
        /// <returns></returns>
        public async Task Wait(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            _tcsSet.Add(tcs);
            using (cancellationToken.Register(() =>
            {
                if (tcs.TrySetCanceled())
                    _tcsSet.Remove(tcs);
            }))
            {
                await tcs.Task;
            }
        }

        private readonly HashSet<TaskCompletionSource<bool>> _tcsSet = new HashSet<TaskCompletionSource<bool>>();
    }
}