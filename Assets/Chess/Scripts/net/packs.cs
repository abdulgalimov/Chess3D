using System;

namespace ChessGame
{
    public class ReceivePack
    {
        public string action;
        public int id;
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
            return String.Format("MovePack from: {0}, to: {1}", from, to);
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
