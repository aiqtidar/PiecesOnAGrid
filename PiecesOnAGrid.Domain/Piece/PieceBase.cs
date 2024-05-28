using PiecesOnAGrid.Domain.Board;

namespace PiecesOnAGrid.Domain.Piece
{
    public abstract class PieceBase : IComparable<PieceBase>
    {
        public required string Name { get; set; }

        public PieceBase(string name)
        {
            if (name == null) throw new NullReferenceException("Name cannot be null for a piece");

            Name = name;
        }

        public int CompareTo(PieceBase? other)
        {
            if (other == null) throw new NullReferenceException("Comparison with Null Chess piece");
            return Name.CompareTo(other.Name);
        }

        public abstract IEnumerable<(int row, int col)> GetMoves((int row, int col) location, int rowBound, int colBound);

        public override string ToString()
        {
            return Name;
        }
    }
}
