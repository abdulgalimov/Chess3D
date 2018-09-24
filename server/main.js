/**
 * Created by Zaur abdulgalimov@gmail.com on 19.09.2018.
 */


const EventEmitter = require('events');
const servers = require('./servers');
servers.onConnect(onConnect);


setInterval(onGameTick, 500);
function onGameTick() {
  const time = Date.now();
  for (let i=0; i<gamesList.length; i++) {
    gamesList[i].update(time);
  }
}

function onConnect(socket) {
  const client = new Client(socket);
  addTurn(client);
}

const turn = [];
function addTurn(client) {
  turn.push(client);
  //
  client.send(Actions.PING);
  if (turn.length > 1) {
    startGame(turn.shift(), turn.shift());
  } else {
    client.send(Actions.TURN);
  }
}

let ClientIdCount = 1;
class Client extends EventEmitter {
  constructor(socket) {
    super();
    //
    this.id = ClientIdCount++;
    this.log('connected');
    this.socket = socket;
    this.opp = null;
    this.game = null;
    socket.onMessage = this.onMessage.bind(this);
    socket.onClose = this.onClose.bind(this);
  }
  
  log() {
    const arr = ['['+this.id+']'].concat([].slice.apply(arguments));
    console.log.apply(console, arr);
  }

  onMessage(event) {
    if (!this.game) return;
    this.log('onMessage', event);
    switch (event.action) {
      case Actions.MOVE:
        this.game.doMove(this, event.data);
        break;
    }
  }

  onClose() {
    const index = turn.indexOf(this);
    if (index !== -1) {
      turn.splice(index, 1);
    }
    //
    this.log('onClose');
    this.closed = true;
    this.emit('onClose');
  }

  close() {
    this.log('close');
    try {
      this.socket.close();
    } catch(e) {
      
    }
  }

  send(action, data, game) {
    const pack = {
      action: action,
      id: this.id,
      data: data ? JSON.stringify(data) : null,
      game: game
    };
    this.log('send', pack);
    this.socket.sendData(pack);
  }
}

const gamesList = [];
function startGame(c1, c2) {
  const game = new Game(c1, c2);
  gamesList.push(game);
}

const TIME_ON_TURN = 30000;

const Actions = {
  PING: 'ping',
  TURN: 'turn',
  INIT: 'init',
  MOVE: 'move',
  GAME: 'game',
};
class Game {
  constructor(c1, c2) {
    c1.opp = c2;
    c1.color = 1;
    //
    c2.opp = c1;
    c2.color = 2;
    //
    this.model = {
      turn: 1
    };
    //
    this.client1 = c1;
    this.client2 = c2;
    const clients = this.clients = [c1, c2];
    const self = this;
    clients.forEach(function(client) {
      client.game = self;
      client.on('onClose', function() {
        self.onClose(client);
      });
    });
    //
    this.sendInit();
  }

  onClose(client) {
    if (client.opp && !client.opp.closed) {
      client.opp.close();
    }
    const index = gamesList.indexOf(this);
    if (index !== -1) {
      gamesList.splice(index, 1);
    }
  }
  
  restartTimer() {
    this.nextTime = Date.now() + TIME_ON_TURN;
  }
  update(now) {
    if (now > this.nextTime) {
      this.model.turn = 3-this.model.turn;
      this.sendTimeout();
      this.restartTimer();
    }
  }
  
  send(client, action, data) {
    client.send(action, data, this.model);
  }

  sendInit() {
    const time = Date.now();
    this.send(this.client1, Actions.INIT, {
      color: this.client1.color,
      stime: time
    });
    this.send(this.client2, Actions.INIT, {
      color: this.client2.color,
      stime: time
    });
    //
    this.restartTimer();
  }
  sendTimeout() {
    this.send(this.client1, Actions.GAME);
    this.send(this.client2, Actions.GAME);
  }

  doMove(client, moveData) {
    this.model.turn = 3-this.model.turn;
    //
    this.send(client, Actions.GAME);
    //
    moveData.from.x = 7-moveData.from.x;
    moveData.from.y = 7-moveData.from.y;
    moveData.to.x = 7-moveData.to.x;
    moveData.to.y = 7-moveData.to.y;
    this.send(client.opp, Actions.MOVE, moveData);
    //
    this.restartTimer();
  }
}
