using System;
using UnityEngine;

namespace ChessGame
{
    public class Position
    {
        public int x;
        public int y;
        
        public Position(int x=0, int y=0)
        {
            this.x = x;
            this.y = y;
        }

        public void Update(Position p)
        {
            x = p.x;
            y = p.y;
        }
        public void Update(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Compare(Position p)
        {
            return x == p.x && y == p.y;
        }

        public Position Clone()
        {
            return new Position(x, y);
        }

        public override string ToString()
        {
            return String.Format("Position {0}x{1}", x, y);
        }
        
    }
    
    public class Coord
    {
        public static Position gameToModel(Vector3 point)
        {
            int x = (int)Math.Round((point.x-1)/9)+4;
            int y = (int)Math.Round((point.z-1)/9)+4;
            x = Math.Max(0, Math.Min(7, x));
            y = Math.Max(0, Math.Min(7, y));
            //
            return new Position(x, y);
        }

        public static Vector3 modelToGame(Position pos, float y = 13)
        {
            return new Vector3((pos.x-4)*9+1.5f, y, (pos.y-4)*9);            
        }
    }
}
