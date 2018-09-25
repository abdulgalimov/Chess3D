using System;
using DG.Tweening;
using UnityEngine;

namespace ChessGame.camera
{
    public class CKnightAttackController : CameraController
    {
        public CKnightAttackController()
        {
            
        }

        public void Init(Piece fromPiece, Vector3 toPoint)
        {
            Vector3 direction = toPoint - fromPiece.transform.position;
            direction.Normalize();
            direction = direction*20;
            Vector3 pos = new Vector3(fromPiece.transform.position.x, 40, fromPiece.transform.position.z) - direction;
            //
            float dir = (float)(Math.Atan2(toPoint.x-fromPiece.transform.position.x, toPoint.z-fromPiece.transform.position.z)*180/Math.PI);
            Vector3 rotateVec = new Vector3(30, dir, 0);
            //
            camera.transform.DORotate(rotateVec, 0.6f).SetEase(Ease.OutQuad);
            camera.transform.DOMove(pos, 0.4f).SetEase(Ease.OutQuad)
                .onComplete += () =>
            {
                emit(CameraControllerEvents.ON_COMPLETE);
            };
        }

        public override void Start()
        {
            
        }
        
        public override void Stop()
        {
            camera.transform.DOMove(new Vector3(-4, 74, -65), 0.3f).SetEase(Ease.OutQuad);
            camera.transform.DORotate(new Vector3(49, 0, 0), 0.6f).SetEase(Ease.OutQuad);            
        }
    }
}
