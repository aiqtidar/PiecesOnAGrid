using Microsoft.Extensions.Configuration;
using PiecesOnAGrid.Domain.Piece;
using System.Text.Json;
using PiecesOnAGrid.Domain.Board;
using PiecesOnAGrid.Service.GameEngineService;

namespace PiecesOnAGrid.UI
{
    public class ConsoleApplication
    {
        private readonly IGameEngine<int> GameEngine;

        public ConsoleApplication(IGameEngine<int> gameEngine)
        {
            GameEngine = gameEngine;
        }

        public void Run(IConfigurationRoot configuration)
        {

            var pieces = LoadPieces(configuration.GetSection("Pieces").GetChildren());
            Board<int> board = GetBoard(configuration.GetSection("Board"));
            int digits = configuration.GetValue<int>("Digits");

            CalculateSolutions(pieces, board, digits);
        }

        private void CalculateSolutions(IEnumerable<PieceBase> pieces, Board<int> board, int digits = 7)
        {
            // Lets output results to the console for now.
            Action<PieceBase, int> outputWriter = ((PieceBase piece, int number) => Console.WriteLine($"Counted {number} results for {piece}"));
            List<Task>? tasks;
            Task t;

            // Solve the problem with bottom up dynamic programming ----------------------------------------------------------------
            SolveWithBottomUpDP(pieces, board, digits, outputWriter, out tasks, out t);

            // Solve the problem with DFS ----------------------------------------------------------------
            SolveWithDFS(pieces, board, digits, outputWriter, out tasks, out t);

        }

        private void SolveWithBottomUpDP(IEnumerable<PieceBase> pieces, Board<int> board, int digits, Action<PieceBase, int> outputWriter, out List<Task>? tasks, out Task t)
        {
            Console.WriteLine("Attempting to solve with bottom up dynamic programming");


            // Get tasks to run in separate threads
            tasks = new List<Task>();
            foreach (var piece in pieces) tasks.Add(Task.Run(() => GameEngine.GetCount(board, piece, digits, outputWriter)));
            t = Task.WhenAll(tasks);

            try
            {
                t.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occured : {e.Message}");
                throw;
            }

            if (t.Status == TaskStatus.RanToCompletion) Console.WriteLine("All dynamic programming solutions completed!");
            else if (t.Status == TaskStatus.Faulted) Console.WriteLine("Some tasks failed");

            Console.WriteLine();
        }

        private void SolveWithDFS(IEnumerable<PieceBase> pieces, Board<int> board, int digits, Action<PieceBase, int> outputWriter, out List<Task> tasks, out Task t)
        {
            Console.WriteLine($"Attempting to solve with DFS");

            // Get tasks to run in separate threads
            tasks = new List<Task>();
            foreach (var piece in pieces) tasks.Add(Task.Run(() => GameEngine.GetCountDFS(board, piece, digits, outputWriter)));
            t = Task.WhenAll(tasks);

            try
            {
                t.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occured : {e.Message}");
                throw;
            }

            if (t.Status == TaskStatus.RanToCompletion) Console.WriteLine("All DFS solutions completed!");
            else if (t.Status == TaskStatus.Faulted) Console.WriteLine("Some tasks failed");
        }

        private Board<int> GetBoard(IConfigurationSection boardConfigs)
        {
            var boardValues = boardConfigs.GetSection("Values").Get<IList<IList<int>>>();
            if (boardValues == null) throw new JsonException("Board values not correct!");

            var unvisitable = ListToTuple(boardConfigs.GetSection("UnvisitableSquares").Get<IList<IList<int>>>());
            var unstartable = ListToTuple(boardConfigs.GetSection("Unstartable").Get<IList<IList<int>>>());

            return new Board<int>(ConvertListTo2DArray(boardValues), unvisitable, unstartable);

            int[,] ConvertListTo2DArray(IList<IList<int>> list)
            {
                if (list == null || list.Count == 0 || list[0].Count == 0) throw new ArgumentException("Null or empty list found!");

                int numRows = list.Count;
                int numCols = list[0].Count;
                int[,] array = new int[numRows, numCols];

                for (int i = 0; i < numRows; i++)
                {
                    if (list[i].Count != numCols) throw new ArgumentException("Inner list length varies!");

                    for (int j = 0; j < numCols; j++) array[i, j] = list[i][j];
                }

                return array;
            }
        }

        private IEnumerable<PieceBase> LoadPieces(IEnumerable<IConfigurationSection> piecesJson)
        {
            if (piecesJson is null) throw new NullReferenceException("Error in Reading Pieces from Config!");

            List<PieceBase> piecesList = new List<PieceBase>();
            foreach (var section in piecesJson)
            {
                var type = section.GetValue<string>("Type");
                IConfigurationSection pieceConfiguration = section.GetSection("Object");

                if (type is null || pieceConfiguration is null || pieceConfiguration.GetValue<string>("Name") == null) throw new JsonException("Unable to read piece configuration");

                // Note: These null references are not accurate and have been taken care of in line of code above
                PieceBase pieceObject = section.GetValue<string>("Type") switch
                {
                    "HoppingPiece" => new HoppingPiece(pieceConfiguration.GetValue<string>("Name"), ListToTuple(pieceConfiguration.GetSection("HoppingOffsets").Get<IList<IList<int>>>())),
                    "DirectionPiece" => new DirectionPiece(pieceConfiguration.GetValue<string>("Name"), ListToTuple(pieceConfiguration.GetSection("Directions").Get<IList<IList<int>>>())),
                    _ => throw new NotImplementedException("Unknown piece type encountered!")
                };

                yield return pieceObject;
            }
        }

        private IList<(int, int)> ListToTuple(IList<IList<int>>? list)
        {
            if (list == null) throw new ArgumentNullException("Null list while parsing configurations");

            IList<(int row, int col)> l = new List<(int row, int col)>();
            foreach (var item in list) l.Add((item[0], item[1]));
            return l;
        }
    }
}
