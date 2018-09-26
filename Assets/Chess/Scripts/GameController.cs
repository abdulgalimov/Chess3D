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
            model.on("changeTurn", onChangeTurn);
            model.reinit();
        }

        private bool _isSelected;
        private void Update()
        {
            if (model.IsMyTurn)
            {
                if (_isSelected)
                {
                    RaycastHit hit;
                    if (getHit(out hit))
                    {
                        move(hit);
                    }
                }
            }
            //
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (getHit(out hit))
                {
                    down(hit);
                }
                _isSelected = true;
            }
            //
            if (Input.GetMouseButtonUp(0))
            {
                _isSelected = false;
                if (model.IsMyTurn)
                {
                    RaycastHit hit;
                    if (getHit(out hit))
                    {
                        up(hit);
                    }
                    model.DisableMy(false);                
                }
            }
        }
        
        // raycast begin ********************************************************************************
        private bool getHit(out RaycastHit hit)
        {
            Ray ray = MainCamera.instance.GetCamera().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                return true;
            }

            return false;
        }
        private void down(RaycastHit hit)
        {
            Piece piece = hit.transform.GetComponent<Piece>();
            if (piece)
            {
                model.SetSelectedPiece(piece);
                return;
            }

            if (!model.IsMyTurn) return;
            //
            model.DisableMy(true);
            board.AuraVisible(model.Selected != null);
            move(hit);
        }

        private void move(RaycastHit hit)
        {
            if (!_isSelected) return;
            //
            Position targetPosition = Coord.gameToModel(hit.point);
            bool valid = model.GetValidMove(targetPosition);
            board.AuraValid(valid);
            board.AuraPosition(targetPosition);
        }

        private void up(RaycastHit hit)
        {
            model.DisableMy(false);
            board.AuraVisible(false);
            //
            Position targetPosition = Coord.gameToModel(hit.point);
            bool valid = model.GetValidMove(targetPosition);
            if (valid)
            {
                net.SendMove(model.Selected.position, targetPosition);
                //
                model.MovePiece(model.Selected.position, targetPosition);
                model.Selected.SetSelected(false);
                model.SetSelectedPiece(null);
            }
        }
        // raycast end ********************************************************************************

        private void changeGame(GamePack gamePack)
        {
            model.CurrentTurn = gamePack.turn;            
        }

        private void onChangeTurn(Event e)
        {
            if (!model.IsMyTurn)
            {
                model.SetSelectedPiece(null);
                hourglass.SetState(HourglassState.OPP_TURN);
            }
            else
            {
                hourglass.SetState(HourglassState.MY_TURN);
            }
        }

        public void NetClosed()
        {
            model.CurrentTurn = PieceColor.NONE;
            MainCamera.instance.WaitMode = true;
            hourglass.SetState(HourglassState.WAIT);
        }

        public void Init(InitPack initPack)
        {
            STime.Init(initPack.serverTime);
            //
            _isSelected = false;
            model.RemoveAll();
            model.reinit();
            //
            MainCamera.instance.WaitMode = false;
            model.MyColor = initPack.color;
            changeGame(initPack.game);
        }
        public void Move(MovePack movePack)
        {
            model.MovePiece(movePack.from, movePack.to);
            changeGame(movePack.game);
        }
        public void UpdateGame(GamePack gamePack) 
        {
            changeGame(gamePack);
        }
    }
}
