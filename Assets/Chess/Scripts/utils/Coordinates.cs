using System;
using UnityEngine;

namespace ChessGame
{
    public class Position
    {
        public int X;
        public int Y;
        
        public Position(int x=0, int y=0)
        {
            X = x;
            Y = y;
        }

        public void Update(Position p)
        {
            X = p.X;
            Y = p.Y;
        }

        public bool Compare(Position p)
        {
            return X == p.X && Y == p.Y;
        }

        public Position Clone()
        {
            return new Position(X, Y);
        }

        public override string ToString()
        {
            return $"Position {X}x{Y}";
        }
        
    }
    
    public static class Coordinates
    {
        public static Position GameToModel(Vector3 point)
        {
            var x = (int)Math.Round((point.x-1)/9)+4;
            var y = (int)Math.Round((point.z-1)/9)+4;
            x = Math.Max(0, Math.Min(7, x));
            y = Math.Max(0, Math.Min(7, y));
            //
            return new Position(x, y);
        }

        public static Vector3 ModelToGame(Position pos, float y = 13)
        {
            return new Vector3((pos.X-4)*9+1.5f, y, (pos.Y-4)*9);            
        }
    }
}
