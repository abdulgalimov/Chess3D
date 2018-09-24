using System;

namespace ChessGame
{
    public class King : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.KING;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            int dx = Math.Abs(position.x - to.x);
            int dy = Math.Abs(position.y - to.y);
            return dx == dy || dx == 0 || dy == 0;
        }
    }
}
