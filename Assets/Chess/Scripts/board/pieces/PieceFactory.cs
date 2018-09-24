using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class PieceConf
    {
        public GameObject gameObject;
        public Piece piece;
        public Collider collider;
    }
    public class PieceFactory
    {
        private static Dictionary<PieceColor, Dictionary<PieceType, GameObject>> prefabs;
        
        private static GameObject loadPiecePrefab(string name)
        {
            return (GameObject)Resources.Load("Marble/"+name, typeof(GameObject));
        }

        private static GameObject board;
        public static void Init(GameObject board)
        {
            PieceFactory.board = board;
            //
            prefabs = new Dictionary<PieceColor, Dictionary<PieceType, GameObject>>();
            prefabs[PieceColor.White] = new Dictionary<PieceType, GameObject>();
            prefabs[PieceColor.Black] = new Dictionary<PieceType, GameObject>();
            //
            prefabs[PieceColor.White][PieceType.PAWN] = loadPiecePrefab("White Pawn");
            prefabs[PieceColor.White][PieceType.ROOK] = loadPiecePrefab("White Rook");
            prefabs[PieceColor.White][PieceType.KNIGHT] = loadPiecePrefab("White Knight");
            prefabs[PieceColor.White][PieceType.BISHOP] = loadPiecePrefab("White Bishop");
            prefabs[PieceColor.White][PieceType.QUEEN] = loadPiecePrefab("White Queen");
            prefabs[PieceColor.White][PieceType.KING] = loadPiecePrefab("White King");
            //
            prefabs[PieceColor.Black][PieceType.PAWN] = loadPiecePrefab("Black Pawn");
            prefabs[PieceColor.Black][PieceType.ROOK] = loadPiecePrefab("Black Rook");
            prefabs[PieceColor.Black][PieceType.KNIGHT] = loadPiecePrefab("Black Knight");
            prefabs[PieceColor.Black][PieceType.BISHOP] = loadPiecePrefab("Black Bishop");
            prefabs[PieceColor.Black][PieceType.QUEEN] = loadPiecePrefab("Black Queen");
            prefabs[PieceColor.Black][PieceType.KING] = loadPiecePrefab("Black King");
        }
        
        public static PieceConf CreatePiece(PieceColor color, PieceType type)
        {
            if (!prefabs[color].ContainsKey(type)) return null;
            //
            GameObject prefab = prefabs[color][type];
            Vector3 posGame = new Vector3();
            GameObject pieceObj = GameObject.Instantiate(prefab, posGame, Quaternion.identity, board.transform);
            if (type == PieceType.KNIGHT)
            {
                pieceObj.transform.Rotate(new Vector3(0, 1, 0), color == PieceColor.White ? 90 : -90);
            }
            //
            Piece piece = null;
            switch (type)
            {
                case PieceType.PAWN:
                    piece = pieceObj.AddComponent<Pawn>();
                    break;
                case PieceType.ROOK:
                    piece = pieceObj.AddComponent<Rock>();
                    break;
                case PieceType.KNIGHT:
                    piece = pieceObj.AddComponent<Knight>();
                    if (color == PieceColor.White)
                    {
                        Knight knight = (Knight) piece;
                        knight.mainCamera = GameController.instance.GetMainCamera();
                    }
                    break;
                case PieceType.BISHOP:
                    piece = pieceObj.AddComponent<Bishop>();
                    break;
                case PieceType.QUEEN:
                    piece = pieceObj.AddComponent<Queen>();
                    break;
                case PieceType.KING:
                    piece = pieceObj.AddComponent<King>();
                    break;
            }
            piece.Color = color;
            piece.Type = type;
            piece.prefab = (GameObject)Resources.Load("FireAura/FirePrefab", typeof(GameObject));
            piece.Start();
            //
            PieceConf conf = new PieceConf();
            conf.gameObject = pieceObj;
            conf.piece = piece;
            //
            if (color == PieceColor.White)
            {
                conf.collider = pieceObj.AddComponent<MeshCollider>();
            }
            return conf;
        }
    }
    
    
}
