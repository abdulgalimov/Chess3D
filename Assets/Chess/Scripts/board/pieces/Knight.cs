using System;
using System.Collections;
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
        
        public override void MoveTo(Position to)
        {
            Debug.LogFormat("MoveTo: {0}", to);
            if (!position.Compare(to))
            {
                position.Update(to);
                Vector3 targetPos = Coord.modelToGame(to);
                if (mainCamera != null)
                {
                    mainCamera.KnightMove(this, targetPos)
                        .onComplete += () =>
                    {
                        playAnim(targetPos);
                    };
                }
                else
                {
                    playAnim(targetPos);
                }
            }
        }

        private void playAnim(Vector3 targetPos)
        {
            Debug.LogFormat("playAnim");
            //
            Vector3 center = new Vector3();
            center.x = transform.position.x + (targetPos.x - transform.position.x) * .8f;
            center.y = 25;
            center.z = transform.position.z + (targetPos.z - transform.position.z) * .8f;
            Vector3[] waypoints =
            {
                center,
                targetPos
            };
            Tween t = transform.DOPath(waypoints, 1.5f, PathType.CatmullRom);
            t.SetEase(Ease.OutQuint);
            //
            //
            float rotate = Color == PieceColor.White ? 90 : -90;
            float dir = (float)(Math.Atan2(targetPos.x-transform.position.x, targetPos.z-transform.position.z)*180/Math.PI)+rotate;
            DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, dir, 0), 0.2f))
                .Append(transform.DORotate(new Vector3(0, dir, -10), 0.6f).SetEase(Ease.OutExpo))
                .Append(transform.DORotate(new Vector3(0, dir, 0), 0.5f).SetEase(Ease.OutExpo))
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
