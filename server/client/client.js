/**
 * Created by Zaur abdulgalimov@gmail.com on 19.09.2018.
 */


console.log('client');

const ws = new WebSocket('ws://abdulgalimov.com:1551');
ws.onopen = function() {
  console.log('on open');
};

ws.onmessage = function(event) {
  const message = JSON.parse(event.data);
  if (message.data) {
    message.data = JSON.parse(message.data);
  }
  console.log('on message', message);
  switch (message.action) {
    case 'init':
      onInit(message);
      break;
  }
};

const model = {};
function onInit(message) {
  model.id = message.id;
  console.log('model.id', model.id);
}

function send(action, data) {
  const pack = {
    action: action,
    data: data ? data : null
  };
  ws.send(JSON.stringify(pack));
}

function move(fromX, fromY, toX, toY) {
  send('move', {
    from: {
      x: fromX,
      y: fromY
    },
    to: {
      x: toX,
      y: toY
    }
  })
}
