using Moq;
using PiecesOnAGrid.Domain.Piece;

namespace PiecesOnAGrid.HoppingPieceTests.Tests
{
    public class PiecesTests
    {
        private HoppingPiece King;

        [SetUp]
        public void Setup()
        {
            // King
            IList<(int row, int col)> KingOffsets = [(1, 0), (-1, 0), (0,1), (0, -1), (1,1), (1,-1), (-1,1), (-1,-1)];
            King = new HoppingPiece("king", KingOffsets);
        }

        [Test]
        public void GetMoves_ReturnsAllMoves_WhenTheyAreAvailable()
        {
            // Piece is centered at 4,4
            HashSet<(int row, int col)> expectedMoves = [(5, 4), (3, 4), (4, 5), (4, 3), (5, 5), (5, 3), (3, 5), (3, 3)];

            foreach (var moves in King.GetMoves((4,4), 10, 10))
            {
                Assert.IsTrue(expectedMoves.Contains(moves));
                expectedMoves.Remove(moves);
            }

            Assert.IsTrue(expectedMoves.Count == 0);
        }

        [Test]
        public void GetMoves_ReturnsAllMoves_WhenSomeAreUnavailable()
        {
            // Piece is centered at 0,0
            HashSet<(int row, int col)> expectedMoves = [(0, 1), (1, 0), (1,1)];

            foreach (var move in King.GetMoves((0, 0), 2, 2))
            {
                Assert.IsTrue(expectedMoves.Contains(move));
                expectedMoves.Remove(move);
            }

            Assert.IsTrue(expectedMoves.Count == 0);
        }

        [Test]
        public void GetMoves_ReturnsNoMoves_WhenNoneAreAvailable()
        {
            // Piece is centered at 0,0
            Assert.IsTrue(King.GetMoves((0, 0), 1, 1).ToList().Count == 0);
        }
    }
}