using ChessGame;
using ChessGame.camera;
using UnityEngine;
using Event = ChessGame.Event;


public sealed class MainCamera : MonoBehaviour
{
    private CMainController main = new CMainController();
    private CGameController game = new CGameController();
    private CKnightAttackController knightAttack = new CKnightAttackController();
    public void Start()
    {
        CameraController.Init(gameObject);
        //
        knightAttack.on(CameraControllerEvents.ON_EXIT, onExit);
        //
        startController(main);
    }

    private void onExit(Event e)
    {
        applyDefault();
    }

    private CameraController current;
    private void startController(CameraController controller)
    {
        if (current != null)
        {
            current.Stop();
        }

        current = controller;
        current.Start();
    }
    
    
    private void Update()
    {
        if (current != null)
        {
            current.Update();
        }
    }

    public CameraController KnightAttack(Piece fromPiece, Vector3 toPoint)
    {
        startController(knightAttack);
        knightAttack.Init(fromPiece, toPoint);
        return knightAttack;
    }


    private bool _waitMode;
    public bool WaitMode
    {
        set
        {
            if (_waitMode != value)
            {
                _waitMode = value;
                applyDefault();
            }
        }
    }

    private void applyDefault()
    {
        if (_waitMode)
        {
            startController(main);                    
        }
        else
        {
            startController(game);
        }
    }

}
