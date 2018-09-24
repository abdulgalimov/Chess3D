using System;
using UnityEngine;

namespace ChessGame
{
    public class STime
    {
        private static double _dtime;
        public static void Init(int serverTime)
        {
            _dtime = DateTime.Now.TimeOfDay.TotalSeconds-serverTime;
        }

        public static DateTime Now
        {
            get { return DateTime.Now.AddMilliseconds(_dtime); }
        }
    }
}
