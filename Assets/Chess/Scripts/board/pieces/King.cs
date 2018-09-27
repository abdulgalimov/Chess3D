using System;

namespace ChessGame
{
    public class King : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.King;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            var dx = Math.Abs(Position.X - to.X);
            var dy = Math.Abs(Position.Y - to.Y);
            return dx == dy || dx == 0 || dy == 0;
        }
    }
}
