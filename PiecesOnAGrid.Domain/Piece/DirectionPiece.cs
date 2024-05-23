using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiecesOnAGrid.Domain.Board;


namespace PiecesOnAGrid.Domain.Piece
{
    public class DirectionPiece : PieceBase
    {

        public required IEnumerable<(int row, int col)> Directions { get; set; }

        public DirectionPiece(string name, IEnumerable<(int row, int col)> directions) : base(name)
        {
            Directions = directions;
        }

        public override IEnumerable<(int row, int col)> GetMoves((int row, int col) location, int rowBound, int colBound)
        {
            foreach (var direction in Directions)
            {
                int i = 1;

                while ((direction.row * i + location.row < rowBound) && (direction.row * i + location.row >= 0) && (direction.col * i + location.col < colBound) && (direction.col * i + location.col >= 0))
                {
                    yield return (direction.row * i + location.row, direction.col * i + location.col);
                    i++;
                }
            }
        }
    }
}
