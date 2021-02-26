using System.Threading;
using System.Threading.Tasks;

namespace GuildMaster.Tools
{
    public static class TaskCompletionSourceExtension
    {
        public static async Task<T> CancellableTask<T>(this TaskCompletionSource<T> taskCompletionSource,
            CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(()=>taskCompletionSource.TrySetCanceled()))
            {
                return await taskCompletionSource.Task;
            }
        }
    }
}