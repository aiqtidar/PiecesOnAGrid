using Moq;
using PiecesOnAGrid.Domain.Piece;
using System.Net.NetworkInformation;

namespace PiecesOnAGrid.Tests.Domain.Tests
{
    public class DirectionPieceTests
    {
        private DirectionPiece Rook;

        [SetUp]
        public void Setup()
        {
            IList<(int row, int col)> directions = [(1, 0), (-1, 0), (0, 1), (0, -1)];
            Rook = new DirectionPiece("Rook", directions);
        }

        [Test]
        public void GetMoves_ReturnsAllMoves_WhenTheyAreAvailable()
        {
            var center = (1, 1);
            HashSet<(int row, int col)> expectedMoves = [(0, 1), (2, 1), (1, 0), (1, 2), (1, 3)];

            foreach (var moves in Rook.GetMoves(center, 3, 4))
            {
                Assert.IsTrue(expectedMoves.Contains(moves));
                expectedMoves.Remove(moves);
            }

            Assert.IsTrue(expectedMoves.Count == 0);
        }

        [Test]
        public void GetMoves_ReturnsAllMoves_WhenSomeAreUnavailable()
        {
            var center = (0, 0);
            HashSet<(int row, int col)> expectedMoves = [(1, 0), (0, 1)];

            foreach (var move in Rook.GetMoves(center, 2, 2))
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
            Assert.IsTrue(Rook.GetMoves((0, 0), 1, 1).ToList().Count == 0);
        }

        [Test]
        public void ToString_ReturnsCorrectString()
        {
            // Piece is centered at 0,0
            Assert.IsTrue(Rook.ToString().CompareTo("Rook") == 0);
        }
    }
}