using System;

namespace ChessGame
{
    public class Queen : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.Queen;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            var dx = Math.Abs(Position.X - to.X);
            var dy = Math.Abs(Position.Y - to.Y);
            return dx < 2 && dy < 2;
        }
    }
}
