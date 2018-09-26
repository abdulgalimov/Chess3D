
using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public class MoveConf
    {
        public Position toPosition;
        public PieceConf fromPiece;
        public PieceConf toPiece;
        public Vector3 toGamePosition;
    }
    
    public class GameModel : EventEmitter
    {
        public static GameModel instance;
        private List<PieceConf> pieces;
        public GameModel()
        {
            instance = this;
        }

        private void createPiece(PieceColor color, PieceType type, int x, int y)
        {
            PieceConf conf = PieceFactory.CreatePiece(color, type);
            if (conf == null) return;
            //
            y = color == PieceColor.White ? y : 7 - y;
            Position posModel = new Position(x, y);
            conf.gameObject.transform.position = Coord.modelToGame(posModel);
            //
            pieces.Add(conf);
        }
        private void createColor(PieceColor color)
        {
            for (int i = 0; i < 8; i++)
            {
                createPiece(color, PieceType.PAWN, i, 1);
            }            
            createPiece(color, PieceType.ROOK, 0, 0);
            createPiece(color, PieceType.ROOK, 7, 0);
            createPiece(color, PieceType.KNIGHT, 1, 0);
            createPiece(color, PieceType.KNIGHT, 6, 0);
            createPiece(color, PieceType.BISHOP, 2, 0);
            createPiece(color, PieceType.BISHOP, 5, 0);
            createPiece(color, PieceType.QUEEN, 3, 0);
            createPiece(color, PieceType.KING, 4, 0);
        }
        public void reinit()
        {
            pieces = new List<PieceConf>();
            //
            createColor(PieceColor.White);
            createColor(PieceColor.Black);            
            
        }

        private PieceColor _myColor;
        public PieceColor MyColor
        {
            set { _myColor = value; }
        }

        public bool IsMyTurn
        {
            get {return _currentTurn != 0 && _currentTurn == _myColor;}
        }

        private PieceColor _currentTurn = 0;
        public PieceColor CurrentTurn
        {
            set
            {
                if (_currentTurn != value)
                {
                    _currentTurn = value;
                    emit("changeTurn");
                }
            }
        }

        private Piece _selected;
        public Piece Selected
        {
            get { return _selected; }
        }

        public void SetSelectedPiece(Piece piece)
        {
            if (_selected != null)
            {
                emit("unselected", _selected);
                _selected.SetSelected(false);
                if (_selected == piece)
                {
                    piece = null;
                }
            }
            //
            _selected = piece;
            if (_selected != null)
            {
                emit("selected", _selected);
                _selected.SetSelected(true);
            }
        }

        private PieceConf GetPieceByPosition(Position position)
        {
            foreach (PieceConf conf in pieces)
            {
                if (conf.piece.position.Compare(position))
                {
                    return conf;
                }
            }
            return null;
        }

        private bool _disableMy;
        public void DisableMy(bool disable)
        {
            if (_disableMy == disable) return;
            _disableMy = disable;
            //
            foreach (PieceConf conf in pieces)
            {
                if (conf.collider != null)
                {
                    conf.collider.enabled = !disable;
                }
            }
        }

        public bool GetValidMove(Position pos)
        {
            if (!_selected || _selected.position.Compare(pos)) return false;
            //
            PieceConf toConf = GetPieceByPosition(pos);
            //
            bool valid = _selected.GetValidMove(pos, toConf!=null ? toConf.piece : null);
            if (!valid) return false;
            //
            if (_selected.Type != PieceType.KNIGHT)
            {
                int dx = pos.x != _selected.position.x ? (pos.x - _selected.position.x)/Math.Abs(pos.x - _selected.position.x) : 0;
                int dy = pos.y != _selected.position.y ? (pos.y - _selected.position.y)/Math.Abs(pos.y - _selected.position.y) : 0;
                Position temp = _selected.position.Clone();
                while (!temp.Compare(pos))
                {
                    temp.x += dx;
                    temp.y += dy;
                    PieceConf conf = GetPieceByPosition(temp);
                    if (conf != null)
                    {
                        if (temp.Compare(pos))
                        {
                            return conf.piece.Color != _myColor;                            
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            //
            return true;
        }
        
        public void MovePiece(Position from , Position to)
        {
            MoveConf moveConf = new MoveConf();
            moveConf.toPosition = to;
            moveConf.fromPiece = GetPieceByPosition(from);
            if (moveConf.fromPiece == null) return;
            //
            moveConf.toPiece = GetPieceByPosition(to);
            if (moveConf.toPiece != null)
            {
                pieces.Remove(moveConf.toPiece);
            }
            moveConf.toGamePosition = Coord.modelToGame(moveConf.toPosition);
            //
            moveConf.fromPiece.piece.MoveTo(moveConf);
        }

        public void RemoveAll()
        {
            foreach (PieceConf piece in pieces)
            {
                piece.piece.Release();
            }
        }
    }
   
}
