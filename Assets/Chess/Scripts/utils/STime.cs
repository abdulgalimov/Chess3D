using System;
using UnityEngine;

namespace ChessGame
{
    public static class STime
    {
        private static double deltaServerTime;
        public static void Init(int serverTime)
        {
            deltaServerTime = DateTime.Now.TimeOfDay.TotalSeconds-serverTime;
        }
    }
}
