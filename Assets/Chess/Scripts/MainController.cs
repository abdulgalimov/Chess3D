using UnityEngine;

namespace ChessGame
{
    public class MainController : MonoBehaviour
    {
        public static MainController Instance;
        
        [SerializeField]
        private GameController game;
        [SerializeField]
        private UIController ui;
        [SerializeField]
        private MainCamera mainCamera;
        [SerializeField]
        private NetController net;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            net.Emitter.On(NetActions.Connected, OnNetConnected);
            net.Emitter.On(NetActions.Closed, OnNetClosed);
            net.Emitter.On(NetActions.Init, OnNetInit);
            net.Emitter.On(NetActions.Move, OnNetMove);
            net.Emitter.On(NetActions.Game, OnNetGame);
            //
            ui.SetStatus("Connection in progress");
            mainCamera.WaitMode = true;
        }

        private void OnNetConnected(Event e)
        {
            ui.SetStatus("Search for an opponent");
        }
        private void OnNetClosed(Event e)
        {
            ui.SetVisible(true);
            ui.SetStatus("Connection aborted");
            game.NetClosed();
        }
        private void OnNetInit(Event e)
        {
            var initPack = (InitPack) e.Data;
            //
            game.Init(initPack);
            ui.SetVisible(false);
        }
        private void OnNetMove(Event e)
        {
            var movePack = (MovePack)e.Data;
            game.Move(movePack);
        }
        private void OnNetGame(Event e)
        {
            var gamePack = (GamePack)e.Data;
            game.UpdateGame(gamePack);
        }
    }
}
