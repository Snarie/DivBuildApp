using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class SynchronizedTaskRunner
    {
        private SemaphoreSlim semaphore;
        private TaskCompletionSource<bool> waitingTaskSource;
        private DateTime lastStartTime;
        private TimeSpan minimumDelay;
        private readonly object lockObject = new object();
        
        public SynchronizedTaskRunner(TimeSpan minDelay)
        {
            semaphore = new SemaphoreSlim(1, 1);
            lastStartTime = DateTime.MinValue;
            minimumDelay = minDelay;
        }

        public async Task<bool> TryEnterAsync()
        {
            //TaskCompletionSource<bool> localTaskSource = null;

            lock (lockObject) 
            {
                if (waitingTaskSource != null)
                {
                    // There's already a task waiting, so exit immediately
                    return false;
                }

                TimeSpan elapsedTime = DateTime.UtcNow - lastStartTime;
                if (elapsedTime < minimumDelay)
                {
                    // Setup delay task since we're within the minimum interval
                    waitingTaskSource = new TaskCompletionSource<bool>();
                    // Calculate the delay needed to satisfy the minimum interval
                    int delayMilliseconds = (int)(minimumDelay - elapsedTime).TotalMilliseconds;
                    DelayStart(delayMilliseconds);
                }
            }
            if (waitingTaskSource != null)
            {
                // Wait for the delay task to be completed
                await waitingTaskSource.Task;
            }

            // Wait for the semaphore to be available
            await semaphore.WaitAsync();
            return true;
        }

        private void DelayStart(int delayMilliseconds)
        {
            Task.Delay(delayMilliseconds).ContinueWith(_ =>
            {
                lock (lockObject)
                {
                    // After the delay, allow the task to proceed
                    waitingTaskSource?.SetResult(true);
                    waitingTaskSource = null;
                }
            });
        }

        public void Release()
        {
            lock (lockObject)
            {
                // Release inside lock to ensure order
                semaphore.Release();
                ResetLastStartTime();
            }
        }

        public void ResetLastStartTime()
        {
            // Record the time when processing starts
            lastStartTime = DateTime.UtcNow;
        }

    }
}
