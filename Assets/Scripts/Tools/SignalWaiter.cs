using System;
using System.Threading;
using System.Threading.Tasks;

namespace GuildMaster.Tools
{
    public class SignalWaiter
    {
        public int WaitersCount { get; private set; } = 0;
        public bool WaiterExist => WaitersCount > 0;

        /// <summary>
        /// 이 객체에 Wait하고 있던 모든 코드가 다음으로 넘어가도록 하고 그 수를 반환함. 
        /// </summary>
        /// <returns></returns>
        public int Signal()
        {
            if (!WaiterExist) return 0;
            var temp = _signalCompletionSource;

            _signalCompletionSource = new TaskCompletionSource<bool>();
            var ret = WaitersCount;
            WaitersCount = 0;

            temp.SetResult(false);
            return ret;
        }

        /// <summary>
        /// Signal이 올 때까지 기다림.
        /// </summary>
        /// <returns></returns>
        public async Task Wait(CancellationToken cancellationToken = default)
        {
            WaitersCount += 1;
            using (cancellationToken.Register(() =>
            {
                WaitersCount -= 1;
            }))
            {
                await _signalCompletionSource.Task;
            }
        }

        private TaskCompletionSource<bool> _signalCompletionSource = new TaskCompletionSource<bool>();
    }
}