using UnityEngine;

namespace ChessGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Board board;
        [SerializeField]
        private Hourglass hourglass;
        [SerializeField]
        private NetController net;
        
        private GameModel model;
        private void Start()
        {
            model = new GameModel();
            //
            PieceFactory.Init(board.gameObject);
            //
            model.On("changeTurn", OnChangeTurn);
            model.Init();
        }

        private bool isDown;
        private void Update()
        {
            if (model.IsMyTurn)
            {
                if (isDown)
                {
                    RaycastHit hit = GetHit();
                    if (hit.transform != null)
                    {
                        MouseMove(hit);
                    }
                }
            }
            //
            if (Input.GetMouseButtonDown(0))
            {
                var hit = GetHit();
                if (hit.transform != null)
                {
                    MouseDown(hit);
                }
                isDown = true;
            }
            //
            if (Input.GetMouseButtonUp(0))
            {
                isDown = false;
                if (model.IsMyTurn)
                {
                    var hit = GetHit();
                    if (hit.transform != null)
                    {
                        MouseUp(hit);
                    }
                    model.DisableMy(false);                
                }
            }
        }
        
        // raycast begin ********************************************************************************
        private static RaycastHit GetHit()
        {
            var ray = MainCamera.Instance.GetCamera().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            return hit;
        }
        private void MouseDown(RaycastHit hit)
        {
            var piece = hit.transform.GetComponent<Piece>();
            if (piece)
            {
                model.SetSelectedPiece(piece);
                return;
            }

            if (!model.IsMyTurn) return;
            //
            model.DisableMy(true);
            board.AuraVisible(model.Selected != null);
            MouseMove(hit);
        }

        private void MouseMove(RaycastHit hit)
        {
            if (!isDown) return;
            //
            var targetPosition = Coordinates.GameToModel(hit.point);
            var valid = model.GetValidMove(targetPosition);
            board.AuraValid(valid);
            board.AuraPosition(targetPosition);
        }

        private void MouseUp(RaycastHit hit)
        {
            model.DisableMy(false);
            board.AuraVisible(false);
            //
            var targetPosition = Coordinates.GameToModel(hit.point);
            var valid = model.GetValidMove(targetPosition);
            if (!valid) return;
            net.SendMove(model.Selected.Position, targetPosition);
            //
            model.MovePiece(model.Selected.Position, targetPosition);
            model.Selected.SetSelected(false);
            model.SetSelectedPiece(null);
        }
        // raycast end ********************************************************************************

        private void ChangeGame(GamePack gamePack)
        {
            model.CurrentTurn = gamePack.turn;            
        }

        private void OnChangeTurn(Event e)
        {
            if (!model.IsMyTurn)
            {
                model.SetSelectedPiece(null);
                hourglass.SetState(HourglassState.OppTurn);
            }
            else
            {
                hourglass.SetState(HourglassState.MyTurn);
            }
        }

        public void NetClosed()
        {
            model.CurrentTurn = PieceColor.None;
            MainCamera.Instance.WaitMode = true;
            hourglass.SetState(HourglassState.Wait);
        }

        public void Init(InitPack initPack)
        {
            STime.Init(initPack.serverTime);
            //
            isDown = false;
            model.RemoveAll();
            model.Init();
            //
            MainCamera.Instance.WaitMode = false;
            model.MyColor = initPack.color;
            ChangeGame(initPack.game);
        }
        public void Move(MovePack movePack)
        {
            model.MovePiece(movePack.from, movePack.to);
            ChangeGame(movePack.game);
        }
        public void UpdateGame(GamePack gamePack) 
        {
            ChangeGame(gamePack);
        }
    }
}
