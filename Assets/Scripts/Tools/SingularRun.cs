using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuildMaster.Tools
{
    /// <summary>
    /// 유일한 Task를 생성함. 하나의 Task가 끝나기 전 다른 Task가 요청되면 그 전 Task를 취소하고 끝날 때까지 기다림.
    /// </summary>
    public class SingularRun
    {
        /// <summary>
        /// 요청된 operation을 처리하고 그 값을 반환함. 끝나기 전 다른 Task가 요청되면 그 전 Task를 취소하고 끝날 때까지 기다림.
        /// </summary>
        /// <param name="operation"> 실행할 일. </param>
        /// <param name="cancellationToken"> cancellationToken </param>
        /// <typeparam name="T"> </typeparam>
        /// <exception cref="OperationCanceledException"> 인수로 준 cancellationToken이 불리거나 다른 Run이 하나 더 불려 작업이 취소됨. </exception>
        /// <returns> operation의 반환값 </returns>
        public async Task<T> Run<T>(Func<CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken = default)
        {
            Task lastTaskCapture;
            do
            {
                lastTaskCapture = _lastTask;
                _lastCts.Cancel();
                try
                {
                    await lastTaskCapture;
                }
                catch
                {
                }
            } while (lastTaskCapture != _lastTask);

            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _lastCts = cts;
            var task = _Run(operation, cts.Token, cancellationToken);
            _lastTask = task;
            return await task;
        }

        /// <summary>
        /// 요청된 operation을 처리함. 끝나기 전 다른 Task가 요청되면 그 전 Task를 취소하고 끝날 때까지 기다림.
        /// </summary>
        /// <param name="operation"> 실행할 일. </param>
        /// <param name="cancellationToken"> cancellationToken </param>
        /// <exception cref="OperationCanceledException"> 인수로 준 cancellationToken이 불리거나 다른 Run이 하나 더 불려 작업이 취소됨. </exception>
        public async Task Run(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
        {
            await Run(async ct =>
            {
                await operation(ct);
                return 0;
            }, cancellationToken);
        }

        public bool Running => !_lastTask.IsCompleted;
        
        // public class CanceledByNewerRunException : OperationCanceledException
        // {
        // }

        private async Task<T> _Run<T>(Func<CancellationToken, Task<T>> operation,
            CancellationToken cancellationToken, CancellationToken givenCancellationToken)
        {
            return await operation(cancellationToken);
        }

        private CancellationTokenSource _lastCts = new CancellationTokenSource();
        private Task _lastTask = Task.CompletedTask;
    }
}