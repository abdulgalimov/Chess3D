
using System;

namespace ChessGame
{
    public class Pawn : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.PAWN;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            if (toPiece)
            {
                int dx = Math.Abs(toPiece.position.x - position.x);
                int dy = toPiece.position.y - position.y;
                return dx == 1 && dy == 1 && toPiece != null && toPiece.Color != Color;
            }
            //
            if (position.x != to.x) return false;
            int delta = to.y - position.y;
            if (position.y <= 2)
            {
                return delta > 0 && delta < 3;
            } else
            {
                return delta == 1;                
            }
        }
    }
}
