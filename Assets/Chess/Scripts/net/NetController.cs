using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ChessGame
{
    public static class NetActions
    {
        public const string Connected = "connected";
        public const string Closed = "closed";
        public const string Init = "init";
        public const string Move = "move";
        public const string Game = "game";
    }
    public class NetController : MonoBehaviour
    {
        public readonly EventEmitter Emitter = new EventEmitter();

        private WebSocket ws;
        private byte[] sendBytes;

        private void Start () {
            Reconnect();
        }

        private void Reconnect()
        {
            connected = false;
#if UNITY_EDITOR
            const string url = "ws://127.0.0.1:1551";
#else
            const string url = "ws://abdulgalimov.com:1551";
#endif
            ws = new WebSocket(new Uri(url));
            MainController.Instance.StartCoroutine(ws.Connect());            
        }

        private bool connected;
        private void Update()
        {
            if (ws.IsClosed())
            {
                Emitter.Emit(NetActions.Closed);
                Reconnect();
                return;
            }

            if (ws.IsConnected() && !connected)
            {
                Emitter.Emit(NetActions.Connected);
                connected = true;
            }
            //
            var receiveData = ws.RecvString();
            if (receiveData != null)
            {
                ParseReceiveData(receiveData);
            }
        }

        private void ParseReceiveData(string receiveData)
        {
            var pack = JsonConvert.DeserializeObject<ReceivePack>(receiveData);
            //
            if (pack.data != null)
            {
                pack.data = Regex.Unescape(pack.data);
            }

            switch (pack.action)
            {
                case NetActions.Init:
                {
                    var initPack = JsonConvert.DeserializeObject<InitPack>(pack.data);
                    initPack.game = pack.game;
                    Emitter.Emit(NetActions.Init, initPack);
                    break;
                }
                case NetActions.Move:
                {
                    var movePack = JsonConvert.DeserializeObject<MovePack>(pack.data);
                    movePack.game = pack.game;
                    Emitter.Emit(NetActions.Move, movePack);
                    break;
                }
                case NetActions.Game:
                    Emitter.Emit(NetActions.Game, pack.game);
                    break;
            }
        }
        
        private void SendData(object data)
        {
            var str = JsonConvert.SerializeObject(data);
            ws.SendString(str);
        }
        


        public void SendMove(Position from, Position to)
        {
            var data = new SendPack {action = "move", data = new MovePack(from, to)};
            SendData(data);
        }
    }
    
    
    /*
    public class NetController : EventEmitter
    {
        public NetController()
        {
            //Connect();
        }

        private TcpClient _tcpClient;
        private NetworkStream _serverStream;
        public void Connect()
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect("127.0.0.1", 1338);
            _serverStream = _tcpClient.GetStream();
        }

        private readonly byte [] _buffer = new byte[1024];
        private int _packSize;
        public void Update()
        {
            if (_tcpClient == null || _serverStream == null || !_serverStream.DataAvailable)
                return;

            if (_packSize == 0)
            {
                _serverStream.Read(_buffer, 0, 1);
                _packSize = _buffer[0];
            }
            else
            {
                int bytesRead = _serverStream.Read(_buffer, 0, _packSize);
                string message = Encoding.UTF8.GetString(_buffer, 0, bytesRead);
                ReceivePack pack = JsonConvert.DeserializeObject<ReceivePack>(message);
                _packSize = 0;
                //
                if (pack.data != null)
                {
                    pack.data = Regex.Unescape(pack.data);
                }
                switch (pack.action)
                {
                    case "move":
                        MovePack movePack = JsonConvert.DeserializeObject<MovePack>(pack.data);
                        emit("move", movePack);
                        break;
                }
            }
        }

        private void sendData(object data)
        {
            string str = JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] pack = new byte[bytes.Length+1];
            pack[0] = (byte) bytes.Length;
            Buffer.BlockCopy(bytes, 0, pack, 1, bytes.Length);
            //
            _serverStream.Write(pack, 0, pack.Length);
        }
        


        public void SendMove(Position from, Position to)
        {
            SendPack data = new SendPack();
            data.action = "move";
            data.data = new MovePack(from, to);
            sendData(data);
        }
    }
    */
}

