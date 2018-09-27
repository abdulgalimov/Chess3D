using System;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public class CKnightAttackController : CameraController
    {
        public void Init(Piece fromPiece, Vector3 toPoint)
        {
            var direction = toPoint - fromPiece.transform.position;
            direction.Normalize();
            direction = direction*20;
            var pos = new Vector3(fromPiece.transform.position.x, 40, fromPiece.transform.position.z) - direction;
            //
            var dir = (float)(Math.Atan2(toPoint.x-fromPiece.transform.position.x, toPoint.z-fromPiece.transform.position.z)*180/Math.PI);
            var rotateVec = new Vector3(30, dir, 0);
            //
            CameraObject.transform.DORotate(rotateVec, 0.6f).SetEase(Ease.OutQuad);
            CameraObject.transform.DOMove(pos, 0.4f).SetEase(Ease.OutQuad)
                .onComplete += () =>
            {
                Emit(CameraControllerEvents.OnComplete);
            };
        }

        public override void Start()
        {   
        }
        
        public override void Stop()
        {
            CameraObject.transform.DOMove(new Vector3(-4, 74, -65), 0.3f).SetEase(Ease.OutQuad);
            CameraObject.transform.DORotate(new Vector3(49, 0, 0), 0.6f).SetEase(Ease.OutQuad);            
        }
    }
}
