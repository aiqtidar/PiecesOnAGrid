namespace PiecesOnAGrid.Domain.Board
{
    // I'm limiting this class to 2D boards. I understand that there can be nD boards designed but it seems to be a stretch for this simple problem.
    public class Board<T>
    {
        private readonly T[,] Values;
        private HashSet<(int row, int col)> UnvisitableSquares = new();
        private HashSet<(int row, int col)> Unstartable = new();

        public Board(T[,] values, IEnumerable<(int, int)>? unvisitableSquares = null, IEnumerable<(int, int)>? unstartableSquares = null)
        {
            Values = values;

            if (unvisitableSquares is not null) foreach (var unvisitable in unvisitableSquares) UnvisitableSquares.Add(unvisitable);
            if (unstartableSquares is not null) foreach (var unstartable in unstartableSquares) Unstartable.Add(unstartable);
        }

        public bool IsSquareVisitable(int row, int col)
        {
            return !UnvisitableSquares.Contains((row, col));
        }

        public bool IsSquareStartable(int row, int col)
        {
            return !Unstartable.Contains((row, col));
        }

        public bool IsSquareWithinBounds(int row, int col)
        {
            return (row >= 0 && row < Values.GetLength(0)) && (col >= 0 && col < Values.GetLength(1));
        }

        public T? GetValue(int row, int col)
        {
            if (!IsSquareWithinBounds(row, col)) throw new ArgumentOutOfRangeException("Either row or column is out of bounds for this board");
            else return Values[row, col];
        }

        public int GetRowBound()
        {
            return Values.GetLength(0);
        }

        public int GetColBound()
        {
            return Values.GetLength(1);
        }
    }
}
