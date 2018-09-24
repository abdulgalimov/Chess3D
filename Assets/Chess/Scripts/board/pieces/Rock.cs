using System;

namespace ChessGame
{
    public class Rock : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.ROOK;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            return position.x == to.x || position.y == to.y;
        }
    }
}
