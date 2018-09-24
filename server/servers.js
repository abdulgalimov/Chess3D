/**
 * Created by Zaur abdulgalimov@gmail.com on 19.09.2018.
 */


function createTcpServer() {
  const net = require('net');

  const server = net.createServer(onConnect);
  server.listen(1552, '127.0.0.1');

  function onConnect(socket) {
    socket.on('data', function(message) {
      message = message.slice(1);
      message = message.toString();
      try {
        message = JSON.parse(message);
      } catch (e) {
        message = {
          text: message
        }
      }
      if (socket.onMessage) socket.onMessage(message);

    });
    socket.sendData = function(data) {
      const str = JSON.stringify(data);
      let pack = new Buffer(1+str.length);
      pack.writeUInt8(str.length);
      pack.write(str, 1);
      socket.write(pack);
      console.log('-------> send', pack.length, str.length, str);
    };
    onConnectCallback(socket);
  }
}


function createWsServer() {
  const WebSocket = require('ws');
  const ws = new WebSocket.Server({
    port: 1551
  });
  ws.on('connection', function(client) {
    client.sendData = function(data) {
      client.send(JSON.stringify(data));
    };
    client.on('message', function(message) {
      if (client.onMessage) client.onMessage(JSON.parse(message));
    });
    client.on('close', function() {
      if (client.onClose) client.onClose();
    });
    onConnectCallback(client);
  });
}

let onConnectCallback;
exports.onConnect = function(callback) {
  onConnectCallback = callback;
  //
  createTcpServer();
  createWsServer();
};
