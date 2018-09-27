
using System;

namespace ChessGame
{
    public class Pawn : Piece
    {
        public override void Start()
        {
            base.Start();
            
            Type = PieceType.Pawn;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            if (toPiece)
            {
                var dx = Math.Abs(toPiece.Position.X - Position.X);
                var dy = toPiece.Position.Y - Position.Y;
                return dx == 1 && dy == 1 && toPiece != null && toPiece.Color != Color;
            }
            
            if (Position.X != to.X) return false;
            var delta = to.Y - Position.Y;
            if (Position.Y <= 2)
            {
                return delta > 0 && delta < 3;
            }
            
            return delta == 1;
        }
    }
}
