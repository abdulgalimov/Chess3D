using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class PieceConf
    {
        public GameObject Object;
        public Piece Piece;
        public Collider Collider;
    }
    public static class PieceFactory
    {
        private static Dictionary<PieceColor, Dictionary<PieceType, GameObject>> prefabs;
        
        private static GameObject LoadPiecePrefab(string name)
        {
            return (GameObject)Resources.Load("Marble/"+name, typeof(GameObject));
        }

        private static GameObject board;
        public static void Init(GameObject boardObject)
        {
            board = boardObject;
            
            prefabs = new Dictionary<PieceColor, Dictionary<PieceType, GameObject>>
            {
                [PieceColor.White] = new Dictionary<PieceType, GameObject>(),
                [PieceColor.Black] = new Dictionary<PieceType, GameObject>(),
                [PieceColor.White] = {[PieceType.Pawn] = LoadPiecePrefab("White Pawn")},
                [PieceColor.White] = {[PieceType.Rook] = LoadPiecePrefab("White Rook")},
                [PieceColor.White] = {[PieceType.Knight] = LoadPiecePrefab("White Knight")},
                [PieceColor.White] = {[PieceType.Bishop] = LoadPiecePrefab("White Bishop")},
                [PieceColor.White] = {[PieceType.Queen] = LoadPiecePrefab("White Queen")},
                [PieceColor.White] = {[PieceType.King] = LoadPiecePrefab("White King")},
                [PieceColor.Black] = {[PieceType.Pawn] = LoadPiecePrefab("Black Pawn")},
                [PieceColor.Black] = {[PieceType.Rook] = LoadPiecePrefab("Black Rook")},
                [PieceColor.Black] = {[PieceType.Knight] = LoadPiecePrefab("Black Knight")},
                [PieceColor.Black] = {[PieceType.Bishop] = LoadPiecePrefab("Black Bishop")},
                [PieceColor.Black] = {[PieceType.Queen] = LoadPiecePrefab("Black Queen")},
                [PieceColor.Black] = {[PieceType.King] = LoadPiecePrefab("Black King")}
            };
        }
        
        public static PieceConf CreatePiece(PieceColor color, PieceType type)
        {
            if (!prefabs[color].ContainsKey(type)) return null;
            
            var prefab = prefabs[color][type];
            var posGame = new Vector3();
            var pieceObj = Object.Instantiate(prefab, posGame, Quaternion.identity, board.transform);
            if (type == PieceType.Knight)
            {
                pieceObj.transform.Rotate(new Vector3(0, 1, 0), color == PieceColor.White ? 90 : -90);
            }
            
            Piece piece = null;
            switch (type)
            {
                case PieceType.Pawn:
                    piece = pieceObj.AddComponent<Pawn>();
                    break;
                case PieceType.Rook:
                    piece = pieceObj.AddComponent<Rock>();
                    break;
                case PieceType.Knight:
                    piece = pieceObj.AddComponent<Knight>();
                    break;
                case PieceType.Bishop:
                    piece = pieceObj.AddComponent<Bishop>();
                    break;
                case PieceType.Queen:
                    piece = pieceObj.AddComponent<Queen>();
                    break;
                case PieceType.King:
                    piece = pieceObj.AddComponent<King>();
                    break;
            }

            if (piece == null) return null;
            
            piece.Color = color;
            piece.Type = type;
            piece.Start();
            
            var conf = new PieceConf {Object = pieceObj, Piece = piece};
            if (color == PieceColor.White)
            {
                conf.Collider = pieceObj.AddComponent<MeshCollider>();
            }
            return conf;
        }
    }
    
    
}
