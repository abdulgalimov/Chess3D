
using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public class MoveConf
    {
        public Position ToPosition;
        public PieceConf FromPiece;
        public PieceConf ToPiece;
        public Vector3 ToGamePosition;
    }
    
    public class GameModel : EventEmitter
    {
        public static GameModel Instance;
        private List<PieceConf> pieces;
        public GameModel()
        {
            Instance = this;
        }

        private void CreatePiece(PieceColor color, PieceType type, int x, int y)
        {
            var conf = PieceFactory.CreatePiece(color, type);
            if (conf == null) return;
            //
            y = color == PieceColor.White ? y : 7 - y;
            var posModel = new Position(x, y);
            conf.Object.transform.position = Coordinates.ModelToGame(posModel);
            //
            pieces.Add(conf);
        }
        private void CreateColor(PieceColor color)
        {
            for (var i = 0; i < 8; i++)
            {
                CreatePiece(color, PieceType.Pawn, i, 1);
            }            
            CreatePiece(color, PieceType.Rook, 0, 0);
            CreatePiece(color, PieceType.Rook, 7, 0);
            CreatePiece(color, PieceType.Knight, 1, 0);
            CreatePiece(color, PieceType.Knight, 6, 0);
            CreatePiece(color, PieceType.Bishop, 2, 0);
            CreatePiece(color, PieceType.Bishop, 5, 0);
            CreatePiece(color, PieceType.Queen, 3, 0);
            CreatePiece(color, PieceType.King, 4, 0);
        }
        public void Init()
        {
            pieces = new List<PieceConf>();
            //
            CreateColor(PieceColor.White);
            CreateColor(PieceColor.Black);            
            
        }

        private PieceColor myColor;
        public PieceColor MyColor
        {
            set { myColor = value; }
        }

        public bool IsMyTurn => currentTurn != 0 && currentTurn == myColor;

        private PieceColor currentTurn = 0;
        public PieceColor CurrentTurn
        {
            set
            {
                if (currentTurn == value) return;
                currentTurn = value;
                Emit("changeTurn");
            }
        }

        public Piece Selected { get; private set; }

        public void SetSelectedPiece(Piece piece)
        {
            if (Selected != null)
            {
                Emit("unselected", Selected);
                Selected.SetSelected(false);
                if (Selected == piece)
                {
                    piece = null;
                }
            }
            //
            Selected = piece;
            if (Selected == null) return;
            Emit("selected", Selected);
            Selected.SetSelected(true);
        }

        private PieceConf GetPieceByPosition(Position position)
        {
            foreach (var conf in pieces)
            {
                if (conf.Piece.Position.Compare(position))
                {
                    return conf;
                }
            }
            return null;
        }

        private bool disableMy;
        public void DisableMy(bool disable)
        {
            if (disableMy == disable) return;
            disableMy = disable;
            //
            foreach (var conf in pieces)
            {
                if (conf.Collider != null)
                {
                    conf.Collider.enabled = !disable;
                }
            }
        }

        public bool GetValidMove(Position pos)
        {
            if (!Selected || Selected.Position.Compare(pos)) return false;
            //
            var toConf = GetPieceByPosition(pos);
            //
            var valid = Selected.GetValidMove(pos, toConf?.Piece);
            if (!valid) return false;
            //
            if (Selected.Type == PieceType.Knight) return true;
            
            var dx = pos.X != Selected.Position.X ? (pos.X - Selected.Position.X)/Math.Abs(pos.X - Selected.Position.X) : 0;
            var dy = pos.Y != Selected.Position.Y ? (pos.Y - Selected.Position.Y)/Math.Abs(pos.Y - Selected.Position.Y) : 0;
            var temp = Selected.Position.Clone();
            while (!temp.Compare(pos))
            {
                temp.X += dx;
                temp.Y += dy;
                var conf = GetPieceByPosition(temp);
                if (conf == null) continue;
                if (temp.Compare(pos))
                {
                    return conf.Piece.Color != myColor;                            
                }
                return false;
            }
            
            return true;
        }
        
        public void MovePiece(Position from , Position to)
        {
            var moveConf = new MoveConf {ToPosition = to, FromPiece = GetPieceByPosition(from)};
            if (moveConf.FromPiece == null) return;
            //
            moveConf.ToPiece = GetPieceByPosition(to);
            if (moveConf.ToPiece != null)
            {
                pieces.Remove(moveConf.ToPiece);
            }
            moveConf.ToGamePosition = Coordinates.ModelToGame(moveConf.ToPosition);
            //
            moveConf.FromPiece.Piece.MoveTo(moveConf);
        }

        public void RemoveAll()
        {
            foreach (var piece in pieces)
            {
                piece.Piece.Release();
            }
        }
    }
   
}
