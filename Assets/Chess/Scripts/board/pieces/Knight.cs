using System;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public class Knight : Piece
    {
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            var dx = Math.Abs(Position.X - to.X);
            var dy = Math.Abs(Position.Y - to.Y);
            return (dx == 2 && dy == 1 || dx == 1 && dy == 2) && (toPiece == null || toPiece.Color != Color);
        }
        
        public override void MoveTo(MoveConf moveConf)
        {
            if (Position.Compare(moveConf.ToPosition)) return;
            
            Position.Update(moveConf.ToPosition);
            if (Color  == PieceColor.White && moveConf.ToPiece != null)
            {
                var cameraController = MainCamera.Instance.KnightAttack(this, moveConf.ToGamePosition);
                cameraController.On(CameraControllerEvents.OnComplete, e =>
                {
                    PlayAnim(moveConf).onComplete += () =>
                    {
                        cameraController.Exit();
                    };                        
                });
            }
            else
            {
                PlayAnim(moveConf);
            }
        }

        private Tween PlayAnim(MoveConf moveConf)
        {
            moveConf.ToPiece?.Piece.Kill(0.2f);
            
            var targetPos = moveConf.ToGamePosition;
            var center = Vector3.zero;
            center.x = transform.position.x + (targetPos.x - transform.position.x) * .7f;
            center.y = 25;
            center.z = transform.position.z + (targetPos.z - transform.position.z) * .7f;
            Vector3[] points =
            {
                center,
                targetPos
            };
            transform
                .DOPath(points, 2.8f, PathType.CatmullRom)
                .SetEase((t, d, c, b) => c*((t=t/d-1)*t*t*t*t+1)+b);
            
            
            var dir = (float)(Math.Atan2(targetPos.x-transform.position.x, targetPos.z-transform.position.z)*180/Math.PI) + 90;
            return DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, dir, 0), 0.2f))
                .Append(transform.DORotate(new Vector3(5, dir, -10), 0.3f).SetEase(Ease.OutExpo))
                .Append(transform.DORotate(new Vector3(0, dir, 0), 0.3f).SetEase(Ease.OutExpo));
        }
    }
}
