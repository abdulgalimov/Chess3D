using System;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public class Knight : Piece
    {
        public MainCamera mainCamera;
        
        public override bool GetValidMove(Position to, Piece toPiece=null)
        {
            int dx = Math.Abs(position.x - to.x);
            int dy = Math.Abs(position.y - to.y);
            return (dx == 2 && dy == 1 || dx == 1 && dy == 2) && (toPiece == null || toPiece.Color != Color);
        }
        
        public override void MoveTo(MoveConf moveConf)
        {
            if (!position.Compare(moveConf.toPosition))
            {
                position.Update(moveConf.toPosition);
                if (mainCamera != null && moveConf.toPiece != null)
                {
                    mainCamera.KnightMove(this, moveConf.toGamePosition)
                        .onComplete += () =>
                    {
                        playAnim(moveConf);
                    };
                }
                else
                {
                    playAnim(moveConf);
                }
            }
        }

        private void playAnim(MoveConf moveConf)
        {
            if (moveConf.toPiece != null)
            {
                moveConf.toPiece.piece.Kill(0.2f);
            }
            //
            Vector3 targetPos = moveConf.toGamePosition;
            Vector3 center = new Vector3();
            center.x = transform.position.x + (targetPos.x - transform.position.x) * .7f;
            center.y = 25;
            center.z = transform.position.z + (targetPos.z - transform.position.z) * .7f;
            Vector3[] waypoints =
            {
                center,
                targetPos
            };
            transform
                .DOPath(waypoints, 2.8f, PathType.CatmullRom)
                .SetEase((t, d, c, b) =>
                {
                    return c*((t=t/d-1)*t*t*t*t+1)+b;
                });
            //
            //
            float rotate = Color == PieceColor.White ? 90 : -90;
            float dir = (float)(Math.Atan2(targetPos.x-transform.position.x, targetPos.z-transform.position.z)*180/Math.PI)+rotate;
            DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, dir, 0), 0.2f))
                .Append(transform.DORotate(new Vector3(5, dir, -10), 0.3f).SetEase(Ease.OutExpo))
                .Append(transform.DORotate(new Vector3(0, dir, 0), 0.3f).SetEase(Ease.OutExpo))
                .onComplete += () =>
            {
                if (mainCamera != null)
                {
                    mainCamera.Reset();
                }
            };            
        }
    }
}
