using System;
using System.Threading;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Blue;

            _ = Game.Random(20).Run(cancellationToken);
            //_ = Game.Blinker().Run(cancellationToken);

            Console.WriteLine("Press any key to stop the game");

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.ReadKey();
                cancellationTokenSource.Cancel();
            }
        }
    }
}
