using System;
using UnityEngine;

namespace ChessGame.camera
{
    public class CMainController : CameraController
    {
        private double time;
        public override void Start()
        {
            time = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }
        public override void Update()
        {
            double delta = DateTime.Now.TimeOfDay.TotalMilliseconds - time;
            time = DateTime.Now.TimeOfDay.TotalMilliseconds;
            cameraParent.Rotate(new Vector3(0, 1, 0), (float)delta/100.0f);
        }
    }
}
