
namespace ChessGame
{
    public class Rock : Piece
    {
        public override void Start()
        {
            base.Start();
            //
            Type = PieceType.Rook;
        }
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            return Position.X == to.X || Position.Y == to.Y;
        }
    }
}
