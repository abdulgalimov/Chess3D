using UnityEngine;

namespace ChessGame
{
    public class GameController : MonoBehaviour
    {
        /**
         * select in editor
         */
        public Board board;
        public Hourglass hourglass;
        
        
        public static GameController instance;
        void Awake()
        {
            instance = this;
        }
        
        
        private Camera camera;
        private MainCamera mainCamera;
        private GameModel model;
        private NetController net;

        private void Start()
        {
            model = new GameModel();
            camera = GetComponent<Camera>();
            mainCamera = camera.GetComponent<MainCamera>();
            //
            PieceFactory.Init(board.gameObject);
            //
            model.on("changeTurn", onChangeTurn);
            model.reinit();
            //
            net = GetComponent<NetController>();
        }

        public MainCamera GetMainCamera()
        {
            return mainCamera;
        }

        private bool _isSelected;
        private void Update()
        {
            if (model.IsMyTurn)
            {
                if (_isSelected)
                {
                    RaycastHit hit;
                    GameObject selector;
                    if (getHit(out hit, out selector))
                    {
                        move(selector, hit);
                    }
                }
            }
            //
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                GameObject selector;
                if (getHit(out hit, out selector))
                {
                    down(selector, hit);
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
                    GameObject selector;
                    if (getHit(out hit, out selector))
                    {
                        up(selector, hit);
                    }
                    model.DisableMy(false);                
                }
            }
        }
        
        // raycast begin ********************************************************************************
        private bool getHit(out RaycastHit hit, out GameObject selector)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                selector = objectHit.gameObject;
                return true;
            }

            selector = null;
            return false;
        }
        private void down(GameObject target, RaycastHit hit)
        {
            Piece piece = target.GetComponent<Piece>();
            if (piece)
            {
                model.SetSelectedPiece(piece);
                return;
            }

            if (!model.IsMyTurn) return;
            //
            model.DisableMy(true);
            board.AuraVisible(model.Selected != null);
            move(target, hit);
        }

        private void move(GameObject target, RaycastHit hit)
        {
            if (!_isSelected) return;
            //
            Position targetPosition = Coord.gameToModel(hit.point);
            bool valid = model.GetValidMove(targetPosition);
            board.AuraValid(valid);
            board.AuraPosition(targetPosition);
        }

        private void up(GameObject target, RaycastHit hit)
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
                //model.Selected.MoveTo(targetPosition);
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
            mainCamera.WaitMode = true;
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
            mainCamera.WaitMode = false;
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
