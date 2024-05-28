using System.Diagnostics.CodeAnalysis;

namespace PiecesOnAGrid.Domain.Piece
{
    // This piece is not really that different from a DirectionPiece now that I've implemented it. The two could possibly be combined into one...
    public class HoppingPiece : PieceBase
    {
        public IEnumerable<(int rowOffset, int colOffset)> HoppingOffsets;

        [SetsRequiredMembers]
        public HoppingPiece(string name, IEnumerable<(int rowOffset, int colOffset)> hoppingOffsets) : base(name)
        {
            HoppingOffsets = hoppingOffsets;
            Name = name;
        }

        public override IEnumerable<(int row, int col)> GetMoves((int row, int col) location, int rowBound, int colBound)
        {
            foreach (var offset in HoppingOffsets)
            {
                (int row, int col) square = (location.row + offset.rowOffset, location.col + offset.colOffset);
                if (square.row >= 0 && square.row < rowBound && square.col >= 0 && square.col < colBound) yield return square;
            }
        }
    }
}
