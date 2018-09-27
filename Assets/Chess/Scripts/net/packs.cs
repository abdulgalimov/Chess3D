
namespace ChessGame
{
    public class ReceivePack
    {
        public string action;
        public string data;
        public GamePack game;
    }

    public class SendPack
    {
        public string action;
        public object data;
    }
    
    public class MovePack
    {
        public MovePack(Position from=null, Position to=null)
        {
            this.from = from;
            this.to = to;
        }
        public Position from;
        public Position to;
        public GamePack game;

        public override string ToString()
        {
            return $"MovePack from: {from}, to: {to}";
        }
    }

    public class InitPack
    {
        public PieceColor color;
        public int serverTime;
        public GamePack game;
    }

    public class GamePack
    {
        public PieceColor turn;
    }
}
