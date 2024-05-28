using PiecesOnAGrid.Domain.Board;
using PiecesOnAGrid.Domain.Piece;

namespace PiecesOnAGrid.Service.GameEngineService
{
    public class GameEngine<TBoardType> : IGameEngine<TBoardType>
    {

        public GameEngine()
        {
        }

        // Approach: Dynamic Programming with memoization
        // The number of possible 1 digit numbers that can be generated from each digit is the number of moves from that digit
        // The number of possible 2 digit numbers that can be generated from each digit is the number of moves from the 1st digit + number of possible moves from each second digit
        // ...
        // The number of possible k digit numbers that can be generated from each digit is the sum of moves from each iteration

        // TODO: Make this async
        public Task<int> GetCount(Board<TBoardType> board, PieceBase piece, int digits = 7, Action<PieceBase, int>? WriteOutput = null)
        {
            if (digits == 0) return Task.FromResult(0);

            var countGrid = new Dictionary<(int row, int col), int>();

            // Base case for 1 digit phone numbers
            for (int r = 0; r < board.GetRowBound(); r++)
            {
                for (int c = 0; c < board.GetColBound(); c++)
                {
                    if (board.IsSquareStartable(r, c) && board.IsSquareVisitable(r, c)) countGrid[(r, c)] = 1;
                }
            }

            if (digits == 1)
            {
                WriteOutput?.Invoke(piece, countGrid.Count);
                return Task.FromResult(countGrid.Count);
            }

            for (int digitNumber = 2; digitNumber <= digits; digitNumber++)
            {
                var currentGridCopy = new Dictionary<(int row, int col), int>();

                foreach (var key in countGrid.Keys)
                {
                    foreach (var move in piece.GetMoves((key.row, key.col), board.GetRowBound(), board.GetColBound()))
                    {
                        if (board.IsSquareVisitable(move.row, move.col))
                        {
                            if (!currentGridCopy.ContainsKey((move.row, move.col))) currentGridCopy[(move.row, move.col)] = 0;
                            currentGridCopy[(move.row, move.col)] += countGrid[(key.row, key.col)];
                        }
                    }
                }
                countGrid = currentGridCopy;
            }

            var res = countGrid.Sum(kv => kv.Value);

            WriteOutput?.Invoke(piece, res);

            return Task.FromResult(res);
        }

    }
}
