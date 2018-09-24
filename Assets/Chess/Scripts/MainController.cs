using UnityEngine;

namespace ChessGame
{
    public class MainController : MonoBehaviour
    {
        /**
         * select in editor
         */
        public Canvas canvas;
        
        private NetController net;
        private GameController game;
        private UIController ui;
        private MainCamera camera;

        private void Start()
        {
            net = GetComponent<NetController>();
            net.emitter.on(NetActions.Connected, onNetConnected);
            net.emitter.on(NetActions.Closed, onNetClosed);
            net.emitter.on(NetActions.Init, onNetInit);
            net.emitter.on(NetActions.Move, onNetMove);
            net.emitter.on(NetActions.Game, onNetGame);
            //
            game = GetComponent<GameController>();
            ui = canvas.GetComponent<UIController>();
            camera = GetComponent<MainCamera>();
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
