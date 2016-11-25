(function() {
  var chartsData = {};

  var paintData = function() {
    getData(function(loadedData) {
      chartsData = loadedData;
      gaugeVisualization(chartsData.primaryRating);
      barVisualization(chartsData.emotions);
      pieVisualization(chartsData.sex);
    });
  };

  paintData();

  var refreshBtn = document.getElementById('refresh-btn');
  refreshBtn.addEventListener('click', paintData);

  // создать подключение
  var socket = new WebSocket("ws://localhost:8081");

  // обработчик входящих сообщений
  socket.onmessage = function(event) {
    console.log(event.data);
    // paintData();
  };

  socket.onopen = function() {
    console.log('Open connection');
  };

  socket.onclose = function() {
    console.log('Close connection');
  };
}());
