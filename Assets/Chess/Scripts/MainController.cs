using UnityEngine;

namespace ChessGame
{
    public class MainController : MonoBehaviour
    {
        public static MainController instance;
        
        [SerializeField]
        private GameController game;
        [SerializeField]
        private UIController ui;
        [SerializeField]
        private MainCamera camera;
        [SerializeField]
        private NetController net;
        
        void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            net.emitter.on(NetActions.Connected, onNetConnected);
            net.emitter.on(NetActions.Closed, onNetClosed);
            net.emitter.on(NetActions.Init, onNetInit);
            net.emitter.on(NetActions.Move, onNetMove);
            net.emitter.on(NetActions.Game, onNetGame);
            //
            ui.SetStatus("Connection in progress");
            camera.WaitMode = true;
        }

        private void onNetConnected(Event e)
        {
            ui.SetStatus("Search for an opponent");
        }
        private void onNetClosed(Event e)
        {
            ui.SetVisible(true);
            ui.SetStatus("Connection aborted");
            game.NetClosed();
        }
        private void onNetInit(Event e)
        {
            InitPack initPack = (InitPack) e.Data;
            //
            game.Init(initPack);
            ui.SetVisible(false);
        }
        private void onNetMove(Event e)
        {
            MovePack movePack = (MovePack)e.Data;
            game.Move(movePack);
        }
        private void onNetGame(Event e)
        {
            GamePack gamePack = (GamePack)e.Data;
            game.UpdateGame(gamePack);
        }
    }
}
