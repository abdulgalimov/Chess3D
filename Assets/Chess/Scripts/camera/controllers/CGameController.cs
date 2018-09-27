using System;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public class CGameController : CameraController
    {
        public override void Start()
        {
            CameraParent.DORotate(new Vector3(0, 0, 0), 0.5f);
        }
        public override void Update()
        {
            var wheel = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(wheel) > 0.0001f)
            {
                CameraParent.Rotate(new Vector3(0, 1, 0), wheel*10);
            }
        }
    }
}
