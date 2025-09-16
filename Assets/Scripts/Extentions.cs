using System.Threading;

namespace MyExtensions
{
    public static class CancellationTokenSourceExtensions
    {
        public static CancellationTokenSource Create(this CancellationTokenSource cts)
        {
            cts?.Dispose();
            return new CancellationTokenSource();
        }
    }
}