namespace KasperskyTestTask
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public static class Program
    {
        /// <summary>
        ///     Метод для вывода всех пары чисел, которые в сумме равны заданному числу
        /// </summary>
        /// <param name="numbers">Массив чисел</param>
        /// <param name="sum">Заданная сумма</param>
        /// <returns></returns>
        private static List<Pair> GetPairs(List<int> numbers, int sum)
        {
            var pairs = new List<Pair>();
            numbers.Sort();
            foreach (var number in numbers)
            {
                var secondNumber = sum - number;
                if (numbers.Any(n => n == secondNumber))
                {
                    pairs.Add(new Pair(number, secondNumber));
                }
            }

            return pairs;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("1-задание");
            var customQueue = new CustomQueue<int>();

            Task.Factory.StartNew(() => customQueue.Push(5));
            Task.Factory.StartNew(() => customQueue.Push(6));

            Task.Factory.StartNew(() => customQueue.Pop());
            Task.Factory.StartNew(() => customQueue.Pop());
            Task.Factory.StartNew(() => customQueue.Pop());

            Console.ReadKey();
            Task.Factory.StartNew(() => customQueue.Push(7));
            Console.ReadKey();

            Console.WriteLine("2-задание");
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var pairs = GetPairs(numbers, 10);
            foreach (var pair in pairs)
            {
                Console.WriteLine($"{pair.First}, {pair.Second}");
            }

            Console.ReadKey();
        }
    }

    /// <summary>
    ///     Пользовательская очередь
    /// </summary>
    public class CustomQueue<T>
    {
        private readonly Queue<T> queue;

        private readonly object queueLock;

        public CustomQueue()
        {
            queue = new Queue<T>();
            queueLock = new object();
        }

        /// <summary>
        ///     Удаляет и возвращает объект, находящийся в начале очереди
        /// </summary>
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

        /// <summary>
        ///     Добавляет элемент в очередь
        /// </summary>
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

    /// <summary>
    ///     Пара чисел
    /// </summary>
    public class Pair
    {
        public Pair(int firstNumber, int secondNumber)
        {
            this.First = firstNumber;
            this.Second = secondNumber;
        }

        public int First { get; set; }

        public int Second { get; set; }
    }
}