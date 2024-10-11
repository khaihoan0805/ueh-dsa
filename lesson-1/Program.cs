using System;
using System.Diagnostics;

namespace lesson_1
{
    class Program
    {
        public class Timing
        {
            TimeSpan startingTime;
            TimeSpan duration;
            public Timing()
            {
                startingTime = new TimeSpan(0);
                duration = new TimeSpan(0);
            }
            public void StopTime()
            {
                duration =
                Process.GetCurrentProcess().Threads[0].
                UserProcessorTime.
                Subtract(startingTime);
            }

            public void startTime()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                startingTime =
                Process.GetCurrentProcess().Threads[0].UserProcessorTime;
            }
            public TimeSpan Result()
            {
                return duration;
            }
        }

        public class MinMaxResult
        {
            public int Max { get; set; }
            public int Min { get; set; }
        }

        static T[] Add<T>(T[] a, T[] b)
        {
            return a.Concat(b).ToArray();
        }
        public static T GenericAdd<T>(T a, T b)
        {
            if (typeof(T).IsArray)
            {
                Array aArray = (Array)(object)a;
                Array bArray = (Array)(object)b;
                Array result = Array.CreateInstance(typeof(T).GetElementType(), aArray.Length + bArray.Length);
                Array.Copy(aArray, result, aArray.Length);
                Array.Copy(bArray, 0, result, aArray.Length, bArray.Length);
                return (T)(object)result;
            }

            return (dynamic)a + (dynamic)b;
        }

        public static MinMaxResult FindMinMax(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                throw new ArgumentException("Array must not be null or empty");
            }
            int max = arr[0];
            int min = arr[0];

            for (var index = 1; index < arr.Length; index++)
            {
                if (arr[index] > max)
                {
                    max = arr[index];
                }

                if (arr[index] < min)
                {
                    min = arr[index];
                }
            }

            return new MinMaxResult { Min = min, Max = max };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // Console.WriteLine(GenericAdd<int>(1, 2));
            // Console.WriteLine(GenericAdd<string>("Khai", "Hoan"));
            // Console.WriteLine(string.Join(", ", GenericAdd<int[]>([1, 2, 3], [3, 4, 5])));

            int[] array = new int[1000];

            Random random = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(501, 5000);
            }

            Timing tObj = new Timing();
            tObj.startTime();

            MinMaxResult result = FindMinMax(array);

            tObj.StopTime();
            Console.WriteLine("time (.NET):" + tObj.Result().TotalSeconds);

            Console.WriteLine($"Max: {result.Max}, Min: {result.Min}");
        }
    }
}