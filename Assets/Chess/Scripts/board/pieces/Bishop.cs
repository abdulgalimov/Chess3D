using System;

namespace ChessGame
{
    public class Bishop : Piece
    {
        public override void Start()
        {
            base.Start();
            
            Type = PieceType.Bishop;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            var dx = Math.Abs(Position.X - to.X);
            var dy = Math.Abs(Position.Y - to.Y);
            return dx == dy;
        }
    }
}
