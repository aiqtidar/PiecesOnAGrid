﻿using PiecesOnAGrid.Domain.Board;
using PiecesOnAGrid.Domain.Piece;

namespace PiecesOnAGrid.Service.GameEngineService
{
    public interface IGameEngine<TBoardType>
    {
        public int GetCount(Board<TBoardType> board, PieceBase piece, int digits, Action<PieceBase, int>? WriteOutput = null);


    }
}