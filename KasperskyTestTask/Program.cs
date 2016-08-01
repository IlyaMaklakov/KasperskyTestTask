namespace KasperskyTestTask
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Program
    {
        static void Main(string[] args)
        {
            var customQueue = new CustomQueue<int>();

            Task.Factory.StartNew(() => customQueue.Push(5));
            Task.Factory.StartNew(() => customQueue.Push(6));

            Task.Factory.StartNew(() => customQueue.Pop());
            Task.Factory.StartNew(() => customQueue.Pop());
            Task.Factory.StartNew(() => customQueue.Pop());

            Console.ReadKey();
            Task.Factory.StartNew(() => customQueue.Push(7));
            Console.ReadKey();
        }
    }

    public class CustomQueue<T>
    {
        private readonly Queue<T> queue;

        private readonly object queueLock;

        public CustomQueue()
        {
            queue = new Queue<T>();
            queueLock = new object();
        }

        public T Pop()
        {
            lock (queueLock)
            {
                while (queue.Count == 0)
                {
                    Console.WriteLine($" Thread {Thread.CurrentThread.ManagedThreadId} wait new pushed element");
                    Monitor.Wait(queueLock);
                }
                var item = queue.Dequeue();
                Console.WriteLine($"{item} popped in thread {Thread.CurrentThread.ManagedThreadId}");

                return item;
            }
        }

        public void Push(T item)
        {
            lock (queueLock)
            {
                queue.Enqueue(item);
                Console.WriteLine($"{item} pushed in thread {Thread.CurrentThread.ManagedThreadId}");
                Monitor.PulseAll(queueLock);
            }
        }
    }
}