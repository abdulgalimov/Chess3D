
using UnityEngine;


namespace ChessGame
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera Instance;
        private readonly CMainController main = new CMainController();
        private readonly CGameController game = new CGameController();
        private readonly CKnightAttackController knightAttack = new CKnightAttackController();
    
        [SerializeField]
        private Camera cameraObject;

        public void Start()
        {
            Instance = this;
            
            CameraController.Init(cameraObject);
            
            knightAttack.On(CameraControllerEvents.OnExit, OnExit);
            
            StartController(main);
        }

        private void OnExit(Event e)
        {
            ApplyDefault();
        }

        private CameraController current;
        private void StartController(CameraController controller)
        {
            current?.Stop();

            current = controller;
            current.Start();
        }
    
    
        private void Update()
        {
            current?.Update();
        }

        public Camera GetCamera()
        {
            return cameraObject;
        }

        public CameraController KnightAttack(Piece fromPiece, Vector3 toPoint)
        {
            StartController(knightAttack);
            knightAttack.Init(fromPiece, toPoint);
            return knightAttack;
        }


        private bool waitMode;
        public bool WaitMode
        {
            set
            {
                if (waitMode == value) return;
                waitMode = value;
                ApplyDefault();
            }
        }

        private void ApplyDefault()
        {
            if (waitMode)
            {
                StartController(main);                    
            }
            else
            {
                StartController(game);
            }
        }

    }

}
