using System;

namespace ChessGame
{
    public class Queen : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.QUEEN;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            int dx = Math.Abs(position.x - to.x);
            int dy = Math.Abs(position.y - to.y);
            return dx < 2 && dy < 2;
        }
    }
}
