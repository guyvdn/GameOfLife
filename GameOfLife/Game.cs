using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Game
    {
        private readonly int _size;
        private bool[,] _grid;
        private int _generation = 1;
        private static readonly Random Rnd = new Random();

        private Game(int size)
        {
            _size = size;
            _grid = new bool[size, size];
        }

        public static Game Random(int size = 20)
        {
            var game = new Game(size);

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    game._grid[x, y] = Rnd.Bool();
                }
            }

            return game;
        }

        public static Game Blinker()
        {
            var game = new Game(5);
            game._grid[2, 1] = true;
            game._grid[2, 2] = true;
            game._grid[2, 3] = true;
            return game;
        }

        private void Print()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Generation: {_generation}");
            Console.WriteLine(new string('-', _size*2));

            for (var x = 0; x < _size; x++)
            {
                for (var y = 0; y < _size; y++)
                {
                    Console.Write(_grid[x, y] ? " ☺" : "  ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('-', _size * 2));
        }

        private bool NextGeneration()
        {
            var gameIsStale = true;
            var newGrid = new bool[_size, _size];
            _generation++;

            for (var x = 0; x < _size; x++)
            {
                for (var y = 0; y < _size; y++)
                {
                    var livingNeighbors = GetLivingNeighbors(x, y);
                    if (_grid[x, y] && (livingNeighbors < 2 || livingNeighbors > 3))
                    {
                        newGrid[x, y] = false;
                        gameIsStale = false;
                    }
                    else if (!_grid[x, y] && livingNeighbors == 3)
                    {
                        newGrid[x, y] = true;
                        gameIsStale = false;
                    }
                    else
                    {
                        newGrid[x, y] = _grid[x, y];
                    }
                }
            }

            _grid = newGrid;

            return !gameIsStale;
        }

        private int GetLivingNeighbors(int x, int y)
        {
            var livingNeighbors = 0;

            for (var neighborX = x - 1; neighborX <= x + 1; neighborX++)
            {
                if (neighborX < 0 || neighborX >= _size)
                    continue;

                for (var neighborY = y - 1; neighborY <= y + 1; neighborY++)
                {
                    if (neighborY < 0 || neighborX == x && neighborY == y || neighborY >= _size)
                        continue;

                    livingNeighbors += _grid[neighborX, neighborY] ? 1 : 0;
                }
            }

            return livingNeighbors;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            do
            {
                Print();
                await Task.Delay(200, cancellationToken);
            } while (!cancellationToken.IsCancellationRequested && NextGeneration());

            Console.WriteLine("Game stopped                                        ");
        }
    }
}