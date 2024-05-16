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
        private readonly SemaphoreSlim semaphore;
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

        public async Task ExecuteAsync(Func<Task> taskFunc)
        {
            if (await TryEnterAsync())
            {
                try
                {
                    ResetLastStartTime();
                    await taskFunc();
                }
                finally
                {
                    Release();
                }
            }
            else
            {
                _ = Logger.LogInfo("Exiting early due to queue");
            }
        }

        public async Task<bool> TryEnterAsync()
        {
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

    

    internal class SynchronizedGroupedTaskRunner<TEnum> where TEnum : Enum
    {
        public SemaphoreSlim GlobalSemaphore;
        public Dictionary<TEnum, SynchronizedTaskRunner> Runners;

        public SynchronizedGroupedTaskRunner(TimeSpan minimumDelay, int initialCount = 1, int maxCount = 1)
        {
            GlobalSemaphore = new SemaphoreSlim(initialCount, maxCount);
            Runners = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToDictionary(T => T, T => new SynchronizedTaskRunner(minimumDelay));
        }

        public async Task ExecuteTaskAsync(TEnum T, Func<Task> taskFunc)
        {
            var runner = Runners[T];
            // Try to enter the item-specific runner first.
            if (await runner.TryEnterAsync())
            {
                // Wait on the global semaphore only after successfully entering.
                await GlobalSemaphore.WaitAsync();
                try
                {
                    runner.ResetLastStartTime();
                    await taskFunc(); // Execute the task.
                }
                finally
                {
                    GlobalSemaphore.Release();
                    runner.Release();
                }
            }
            else
            {
                // Log if unable to enter runner
                _ = Logger.LogInfo("Exiting early due to queue for " + T);
            }
        }
    }

    internal class SynchronizedIndexGroupedTaskRunner<TEnum> where TEnum : Enum
    {
        public SemaphoreSlim GlobalSemaphore;
        public Dictionary<(TEnum, int), SynchronizedTaskRunner> Runners;

        public SynchronizedIndexGroupedTaskRunner(TimeSpan minimumDelay, int indexAmount, int initialCount = 1, int maxCount = 1)
        {
            GlobalSemaphore = new SemaphoreSlim(initialCount, maxCount);

            Runners = new Dictionary<(TEnum, int), SynchronizedTaskRunner>();
            foreach (TEnum T in Enum.GetValues(typeof(TEnum)))
            {
                for (int i = 0; i < indexAmount; i++)
                {
                    Runners[(T, i)] = new SynchronizedTaskRunner(minimumDelay);
                }
            }
        }

        public async Task ExecuteTaskAsync(TEnum T, int index, Func<Task> taskFunc)
        {
            var key = (T, index);
            if (!Runners.ContainsKey(key))
            {
                throw new ArgumentException($"No runner available for type {T} at index {index}.");
            }
            var runner = Runners[key];
            // Try to enter the item-specific runner first.
            if (await runner.TryEnterAsync())
            {
                // Wait on the global semaphore only after successfully entering.
                await GlobalSemaphore.WaitAsync();
                try
                {
                    runner.ResetLastStartTime();
                    await taskFunc(); // Execute the task.
                }
                finally
                {
                    GlobalSemaphore.Release();
                    runner.Release();
                }
            }
            else
            {
                // Log if unable to enter runner
                _ = Logger.LogInfo("Exiting early due to queue for " + T);
            }
        }
    }
}
