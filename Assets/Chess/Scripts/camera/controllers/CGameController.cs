using System;
using DG.Tweening;
using UnityEngine;

namespace ChessGame.camera
{
    public class CGameController : CameraController
    {
        public CGameController()
        {
            
        }

        public override void Start()
        {
            cameraParent.DORotate(new Vector3(0, 0, 0), 0.5f);
        }
        public override void Update()
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(wheel) > 0.0001f)
            {
                cameraParent.Rotate(new Vector3(0, 1, 0), wheel*10);
            }
        }
    }
}
