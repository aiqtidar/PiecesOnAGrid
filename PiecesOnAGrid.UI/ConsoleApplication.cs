using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PiecesOnAGrid.Domain.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using PiecesOnAGrid.Domain.Board;
using PiecesOnAGrid.Service.GameEngineService;
using static System.Collections.Specialized.BitVector32;

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

            CalculateSolutions(pieces, board);
        }

        private void CalculateSolutions(IEnumerable<PieceBase> pieces, Board<int> board)
        {
            #region ActualApplicationCode

            // Lets output to the console and the file.
            Action<PieceBase, int> outputWriter = WriteResultToConsole;

            //outputWriter += WriteResultToFile;

            int digits = 7;

            foreach (var piece in pieces)
            {
                GameEngine.GetCount(board, piece, digits, outputWriter);
            };

            #endregion

            #region Helpers

            // Just a custom method to write to a file
            void WriteResultToFile(PieceBase piece, int number)
            {
                string filePath = $"results_{DateTime.Now}.txt";

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath)) writer.WriteLine($"Counted {number} results for {piece}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred while writing to the file: {e.Message}");
                    throw;
                }
            }

            // Just a custom method to write to a file
            void WriteResultToConsole(PieceBase piece, int number)
            {
                Console.WriteLine($"Counted {number} results for {piece}");
            }

            #endregion
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

                PieceBase pieceObject = section.GetValue<string>("Type") switch
                {
                    "HoppingPiece" => new HoppingPiece(pieceConfiguration.GetValue<string>("Name"), ListToTuple(pieceConfiguration.GetSection("HoppingOffsets").Get<IList<IList<int>>>())),
                    "DirectionPiece" => new DirectionPiece(pieceConfiguration.GetValue<string>("Name"), ListToTuple(pieceConfiguration.GetSection("Directions").Get<IList<IList<int>>>())),
                    _ => throw new JsonException("Unknown piece type.")
                };

                yield return pieceObject;
            }
        }

        private IList<(int, int)> ListToTuple(IList<IList<int>> list)
        {
            IList<(int row, int col)> l = new List<(int row, int col)>();
            foreach (var item in list) l.Add((item[0], item[1]));
            return l;
        }
    }
}
